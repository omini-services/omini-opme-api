using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class QuotationRepository : RepositoryDocumentEntity<Quotation>, IQuotationRepository
{
    public QuotationRepository(OpmeContext context) : base(context)
    {
    }

    public override async Task<Quotation?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var quotation = await DbSet.Include(p => p.Items)
                          .Where(p => p.Id == id)
                          .SingleOrDefaultAsync(cancellationToken);

        if (quotation is not null)
        {
            //quotation.PayingSource = new PayingSource() { Name = GetPaymentSource(quotation) };
        }

        return quotation;
    }

    public override async Task<Quotation?> GetByNumber(long number, CancellationToken cancellationToken = default)
    {
        var quotation = await DbSet.Include(p => p.Items).AsNoTracking()
                          .Where(p => p.Number == number)
                          .SingleOrDefaultAsync(cancellationToken);

        if (quotation is not null)
        {
            //quotation.PayingSource = new PayingSource() { Name = GetPaymentSource(quotation) };
        }

        return quotation;
    }

    private string GetPaymentSource(Quotation quotation)
    {
        var payingSourceQuery = quotation.PayingSourceType switch
        {
            PayingSourceType.Hospital => from hospital in Db.Hospitals
                                         where hospital.Code == quotation.PayingSourceCode
                                         select hospital.Name.TradeName,
            PayingSourceType.Patient => from patient in Db.Patients
                                        where patient.Code == quotation.PayingSourceCode
                                        select patient.Name.FullName,
            PayingSourceType.InsuranceCompany => from insuranceCompany in Db.InsuranceCompanies
                                                 where insuranceCompany.Code == quotation.PayingSourceCode
                                                 select insuranceCompany.Name.TradeName,
            PayingSourceType.Physician => from physician in Db.Physicians
                                          where physician.Code == quotation.PayingSourceCode
                                          select physician.Name.FullName,
            _ => throw new ArgumentException(nameof(quotation.PayingSourceType)),
        };

        var payingSource = payingSourceQuery.SingleOrDefault();

        if (payingSource is null)
        {
            throw new InvalidPayingSourceException(quotation.PayingSourceCode.ToString());
        }

        return payingSource;
    }
}


internal class QuotationItemRepository : RepositoryDocumentRowEntity<QuotationItem>, IQuotationItemRepository
{
    public QuotationItemRepository(OpmeContext context) : base(context)
    {
    }
}