using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class PhysicianRepository : RepositoryMasterEntity<Physician>, IPhysicianRepository
{
    public PhysicianRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Physician> Filter(IQueryable<Physician> query, string? queryField, string? queryValue)
    {
        if (queryField is not null && queryValue is not null)
        {
            queryValue = queryValue.ToLower();
            switch (queryField.ToLowerInvariant())
            {
                case var code when code == nameof(Physician.Code).ToLower():
                    query = query.Where(x => x.Code == queryValue);
                    break;

                case var name when name == nameof(Physician.Name).ToLowerInvariant():
                    query = query.Where(x => x.Name.FirstName.ToLower().Contains(queryValue)
                                             || x.Name.MiddleName.ToLower().Contains(queryValue)
                                             || x.Name.LastName.ToLower().Contains(queryValue));
                    break;

                default:
                    return query;
            }
        }

        return query;
    }
}
