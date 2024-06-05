using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class ItemRepository : RepositoryMasterEntity<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }
}
