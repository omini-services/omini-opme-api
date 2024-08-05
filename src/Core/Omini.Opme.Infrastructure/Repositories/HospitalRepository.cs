using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class HospitalRepository : RepositoryMasterEntity<Hospital>, IHospitalRepository
{
    public HospitalRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Hospital> Filter(IQueryable<Hospital> query, string? queryValue = null)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (queryValue is null) return query;

        queryValue = queryValue.ToLower();

        query = query.Where(x => x.Name.LegalName.ToLower().Contains(queryValue)
                                    || x.Name.TradeName.ToLower().Contains(queryValue)
                                    || x.Code.ToLower().Equals(queryValue));

        return query;
    }
}
