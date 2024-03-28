

using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class ItemRepository : IItemRepository
{
    private readonly OpmeContext _context;

    public ItemRepository(OpmeContext context)
    {
        _context = context;
    }

    public async Task Create(Item item)
    {
        await _context.Items.AddAsync(item);
    }

    public async Task<IList<Item>> GetAll()
    {
        return await _context.Items
                                .AsNoTracking().ToListAsync();
    }

    public async Task<Item> GetById(Guid id)
    {
        return await _context.Items
                                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Item item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
