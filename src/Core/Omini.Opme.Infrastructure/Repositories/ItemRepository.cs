using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }

    public async Task<Item?> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().SingleOrDefaultAsync(p => p.Code == code, cancellationToken);
    }
}
