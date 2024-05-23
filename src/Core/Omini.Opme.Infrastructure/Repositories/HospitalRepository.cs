using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class HospitalRepository : Repository<Hospital>, IHospitalRepository
{
    public HospitalRepository(OpmeContext context) : base(context)
    {
    }
}
