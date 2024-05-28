using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Infrastructure.Contexts;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Infrastructure.Repositories;

internal class QuotationRepository : Repository<Quotation>, IQuotationRepository
{
    public QuotationRepository(OpmeContext context) : base(context)
    {
    }

    public override async Task<PagedResult<Quotation>> GetAllPaginated(int pageNumber = default, int pageSize = default, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(p => p.Patient)
                         .Include(p => p.Hospital)
                         .Include(p => p.Physician)
                         .Include(p => p.InsuranceCompany)
                         .OrderBy(p => p.Number)
                         .AsNoTracking();

        return await GetPagedResult(query, pageNumber, pageSize, cancellationToken);

        // var query = DbSet.AsNoTracking()
        //                   .Include(p => p.Items)
        //                   .Include(p => p.Patient)
        //                   .Include(p => p.Hospital)
        //                   .Include(p => p.Physician)
        //                   .Include(p => p.InsuranceCompany);

        // var withPayingSource = from quotation in query
        //                        join hospital in Db.Hospitals on quotation.PayingSourceId equals hospital.Id into hospitalGroup
        //                        from hospital in hospitalGroup.DefaultIfEmpty()
        //                        join patient in Db.Patients on quotation.PayingSourceId equals patient.Id into patientGroup
        //                        from patient in patientGroup.DefaultIfEmpty()
        //                        join insuranceCompany in Db.InsuranceCompanies on quotation.PayingSourceId equals insuranceCompany.Id into insuranceGroup
        //                        from insuranceCompany in insuranceGroup.DefaultIfEmpty()
        //                        join physician in Db.Physicians on quotation.PhysicianId equals physician.Id into physicianGroup
        //                        from physician in physicianGroup.DefaultIfEmpty()
        //                        select new
        //                        {
        //                            quotation,
        //                            payingSource = hospital.Name.TradeName ?? patient.Name.FullName ?? insuranceCompany.Name.LegalName ?? physician.Name.FullName ?? string.Empty
        //                        };

        // foreach (var groupedQuotation in await withPayingSource.ToListAsync())
        // {
        //     groupedQuotation.quotation.PayingSource = new PayingSource() { Name = groupedQuotation.payingSource };
        // }

        //return withPayingSource.Select(p => p.quotation).ToList();
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
