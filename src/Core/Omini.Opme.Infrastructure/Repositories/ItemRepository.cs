using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class ItemRepository : RepositoryMasterEntity<Item>, IItemRepository
{
    public ItemRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<Item> Filter(IQueryable<Item> query, string? queryField, string? queryValue)
    {
        if (queryField is not null && queryValue is not null)
        {
            queryValue = queryValue.ToLower();
            switch (queryField.ToLowerInvariant())
            {
                case var code when code == nameof(Item.Code).ToLower():
                    query = query.Where(x => x.Code == queryValue);
                    break;

                case var name when name == nameof(Item.Name).ToLowerInvariant():
                    query = query.Where(x => x.Name.ToLower().Contains(queryValue)
                                             || x.SalesName.ToLower().Contains(queryValue)
                                             || x.Description.ToLower().Contains(queryValue));
                    break;

                default:
                    return query;
            }
        }

        return query;
    }
}
