using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class PhysicianRepository : Repository<Physician>, IPhysicianRepository
{
    public PhysicianRepository(OpmeContext context) : base(context)
    {
    }
}
