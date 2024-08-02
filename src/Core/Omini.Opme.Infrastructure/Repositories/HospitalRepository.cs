using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class HospitalRepository : RepositoryMasterEntity<Hospital>, IHospitalRepository
{
    public HospitalRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Hospital> Filter(IQueryable<Hospital> query, string? queryField, string? queryValue)
    {
        if (queryField is not null && queryValue is not null)
        {
            queryValue = queryValue.ToLower();
            switch (queryField.ToLowerInvariant())
            {
                case var code when code == nameof(Hospital.Code).ToLower():
                    query = query.Where(x => x.Code == queryValue);
                    break;

                case var name when name == nameof(Hospital.Name.TradeName).ToLowerInvariant() || name == nameof(Hospital.Name.LegalName).ToLowerInvariant():
                    query = query.Where(x => x.Name.LegalName.ToLower().Contains(queryValue)
                                             || x.Name.TradeName.ToLower().Contains(queryValue));
                    break;

                default:
                    return query;
            }
        }

        return query;
    }
}
