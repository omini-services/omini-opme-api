using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllQuotationsQuery : IQuery<Quotation>
{
    public GetAllQuotationsQuery() { }

    public GetAllQuotationsQuery(QueryFilter queryFilter, PaginationFilter paginationFilter)
    {
        QueryFilter = queryFilter;
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public QueryFilter QueryFilter { get; set; }
    public class GetAllQuotationsQueryHandler : IQueryHandler<GetAllQuotationsQuery, Quotation>
    {
        private readonly IQuotationRepository _quotationRepository;
        public GetAllQuotationsQueryHandler(IQuotationRepository quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<PagedResult<Quotation>> Handle(GetAllQuotationsQuery request, CancellationToken cancellationToken)
        {
            var quotations = await _quotationRepository.GetAll(
                currentPage: request.PaginationFilter.CurrentPage,
                pageSize: request.PaginationFilter.PageSize,
                orderByField: request.PaginationFilter.OrderBy,
                sortDirection: request.PaginationFilter.Direction,
                queryField: request.QueryFilter.QueryField,
                queryValue: request.QueryFilter.QueryValue,
                cancellationToken);

            return quotations;
        }
    }
}