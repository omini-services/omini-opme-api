using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }

    public async Task<Item?> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet.SingleOrDefaultAsync(p => p.Code == code, cancellationToken);
    }
}
