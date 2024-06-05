using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllItemsQuery : IQuery<Item>
{
    public GetAllItemsQuery() { }

    public GetAllItemsQuery(QueryFilter queryFilter, PaginationFilter paginationFilter)
    {
        QueryFilter = queryFilter;
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public QueryFilter QueryFilter { get; set; }

    public class GetAllItemsQueryHandler : IQueryHandler<GetAllItemsQuery, Item>
    {
        private readonly IItemRepository _itemRepository;
        public GetAllItemsQueryHandler(IItemRepository itemRepositoryy)
        {
            _itemRepository = itemRepositoryy;
        }

        public async Task<PagedResult<Item>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _itemRepository.GetAll(
                currentPage: request.PaginationFilter.CurrentPage,
                pageSize: request.PaginationFilter.PageSize,
                orderByField: request.PaginationFilter.OrderBy,
                sortDirection: request.PaginationFilter.Direction,
                queryField: request.QueryFilter.QueryField,
                queryValue: request.QueryFilter.QueryValue,
                cancellationToken);

            return items;
        }
    }
}