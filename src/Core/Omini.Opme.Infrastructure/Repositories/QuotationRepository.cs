using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Infrastructure.Contexts;
using Omini.Opme.Shared.Formatters;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class QuotationRepository : RepositoryDocumentEntity<Quotation>, IQuotationRepository
{
    public QuotationRepository(OpmeContext context) : base(context)
    {
    }

    public override async Task<Quotation?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var quotation = await DbSet.Include(p => p.Items)
                          .Where(p => p.Id == id)
                          .SingleOrDefaultAsync(cancellationToken);

        return quotation;
    }

    public override async Task<Quotation?> GetByNumber(long number, CancellationToken cancellationToken = default)
    {
        var quotation = await DbSet.Include(p => p.Items).AsNoTracking()
                          .Where(p => p.Number == number)
                          .SingleOrDefaultAsync(cancellationToken);

        return quotation;
    }

    public override IQueryable<Quotation> Filter(IQueryable<Quotation> query, string? queryValue = null)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (queryValue is null) return query;

        query = query.Where(x => x.PatientName.FullName.ToLower().Contains(queryValue)
                                || x.PhysicianName.FullName.ToLower().Contains(queryValue)
                                || x.HospitalName.ToLower().Contains(queryValue)
                                || x.InsuranceCompanyName.ToLower().Contains(queryValue));

        var digitsValue = queryValue.GetDigits();
        long digitFilter;
        if (long.TryParse(digitsValue, out digitFilter))
        {
            query = query.Where(x => x.Number.Equals(digitFilter));
        }

        return query;
    }
}


internal class QuotationItemRepository : RepositoryDocumentRowEntity<QuotationItem>, IQuotationItemRepository
{
    public QuotationItemRepository(OpmeContext context) : base(context)
    {
    }
}