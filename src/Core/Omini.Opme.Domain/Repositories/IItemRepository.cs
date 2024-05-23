using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Domain.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByCode(string code, CancellationToken cancellationToken = default);
}