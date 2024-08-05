using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class ItemRepository : RepositoryMasterEntity<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Item> Filter(IQueryable<Item> query, string? queryValue = null)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (queryValue is null) return query;

        queryValue = queryValue.ToLower();

        query = query.Where(x => x.Name.ToLower().Contains(queryValue)
             || x.SalesName.ToLower().Contains(queryValue)
             || x.Description.ToLower().Contains(queryValue)
             || x.Code.ToLower().Equals(queryValue));

        return query;
    }
}
