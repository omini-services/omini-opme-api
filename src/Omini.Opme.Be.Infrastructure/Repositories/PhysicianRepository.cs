

using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class PhysicianRepository : Repository<Physician>, IPhysicianRepository
{
    public PhysicianRepository(OpmeContext context) : base(context)
    {
    }
}
