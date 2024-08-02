using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class PatientRepository : RepositoryMasterEntity<Patient>, IPatientRepository
{
    public PatientRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Patient> Filter(IQueryable<Patient> query, string? queryField, string? queryValue)
    {
        if (queryField is not null && queryValue is not null)
        {
            queryValue = queryValue.ToLower();
            switch (queryField.ToLowerInvariant())
            {
                case var code when code == nameof(Patient.Code).ToLower():
                    query = query.Where(x => x.Code == queryValue);
                    break;

                case var name when name == nameof(Patient.Name).ToLowerInvariant():
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
