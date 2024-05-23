using System.Linq.Expressions;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Domain.Services;

public interface IItemService
{
    public Task<Item> Add(Item item, CancellationToken cancellationToken = default);
    public Task<Item> Update(Item item, CancellationToken cancellationToken = default);
    public Task<Item> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<Item> GetById(Guid id, CancellationToken cancellationToken = default);
    public Task<Item> GetByCode(string code, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Item>> Get(Expression<Func<Item, bool>> predicate, CancellationToken cancellationToken = default);
}