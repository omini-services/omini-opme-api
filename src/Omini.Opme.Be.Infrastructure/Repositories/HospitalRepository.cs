

using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class HospitalRepository : Repository<Hospital>, IHospitalRepository
{
    public HospitalRepository(OpmeContext context) : base(context)
    {
    }
}
