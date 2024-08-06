using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class PhysicianRepository : RepositoryMasterEntity<Physician>, IPhysicianRepository
{
    public PhysicianRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Physician> Filter(IQueryable<Physician> query, string? queryValue= null)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (queryValue is null) return query;

        queryValue = queryValue.ToLower();

        query = query.Where(x => x.Name.FirstName.ToLower().Contains(queryValue)
                                 || x.Name.MiddleName.ToLower().Contains(queryValue)
                                 || x.Name.LastName.ToLower().Contains(queryValue)
                                 || x.Code.ToLower().Equals(queryValue));

        return query;
    }
}
