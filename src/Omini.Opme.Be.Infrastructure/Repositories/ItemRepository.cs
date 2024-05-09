using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }
}
