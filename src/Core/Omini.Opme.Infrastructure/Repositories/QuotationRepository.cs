using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Infrastructure;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class QuotationRepository : Repository<Quotation>, IQuotationRepository
{
    public QuotationRepository(OpmeContext context) : base(context)
    {
    }

    public override async Task<List<Quotation>> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
                          .Include(p => p.Items)
                          .ToListAsync(cancellationToken);
    }

    public override async Task<Quotation?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var quotation = await DbSet.Include(p => p.Items)
                          .Include(p => p.Patient)
                          .Include(p => p.Hospital)
                          .Include(p => p.Physician)
                          .Include(p => p.InsuranceCompany)
                          .Where(p => p.Id == id)
                          .SingleOrDefaultAsync(cancellationToken);

        if (quotation is not null)
        {
            quotation.PayingSource = new PayingSource() { Name = GetPaymentSource(quotation) };
        }

        return quotation;
    }

    private string GetPaymentSource(Quotation quotation)
    {
        var payingSourceQuery = quotation.PayingSourceType switch
        {
            PayingSourceType.Hospital => from hospital in Db.Hospitals
                                         where hospital.Id == quotation.PayingSourceId
                                         select hospital.Name.TradeName,
            PayingSourceType.Patient => from patient in Db.Patients
                                        where patient.Id == quotation.PayingSourceId
                                        select patient.Name.FullName,
            PayingSourceType.InsuranceCompany => from insuranceCompany in Db.InsuranceCompanies
                                                 where insuranceCompany.Id == quotation.PayingSourceId
                                                 select insuranceCompany.Name.TradeName,
            PayingSourceType.Physician => from physician in Db.Physicians
                                          where physician.Id == quotation.PayingSourceId
                                          select physician.Name.FullName,
            _ => throw new ArgumentException(nameof(quotation.PayingSourceType)),
        };

        var payingSource = payingSourceQuery.SingleOrDefault();

        if (payingSource is null)
        {
            throw new InvalidPayingSourceException(quotation.PayingSourceId.ToString());
        }

        return payingSource;
    }
}
