using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllQuotationsQuery : IQuery<Quotation>
{
    public GetAllQuotationsQuery() { }

    public GetAllQuotationsQuery(string queryValue, PaginationFilter paginationFilter)
    {
        QueryValue = queryValue;
        PaginationFilter = paginationFilter;
    }

    public string QueryValue { get; set; }
    public PaginationFilter PaginationFilter { get; set; }

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
                queryValue: request.QueryValue,
                cancellationToken);

            return quotations;
        }
    }
}