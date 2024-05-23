using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(OpmeContext context) : base(context)
    {
    }
}
