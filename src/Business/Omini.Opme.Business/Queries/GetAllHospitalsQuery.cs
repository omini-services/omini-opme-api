using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllHospitalsQuery : IQuery<Hospital>
{
    public GetAllHospitalsQuery() { }

    public GetAllHospitalsQuery(string queryValue, PaginationFilter paginationFilter)
    {
        QueryValue = queryValue;
        PaginationFilter = paginationFilter;
    }

    public string QueryValue { get; set; }
    public PaginationFilter PaginationFilter { get; set; }

    public class GetAllHospitalsQueryHandler : IQueryHandler<GetAllHospitalsQuery, Hospital>
    {
        private readonly IHospitalRepository _hospitalRepository;
        public GetAllHospitalsQueryHandler(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public async Task<PagedResult<Hospital>> Handle(GetAllHospitalsQuery request, CancellationToken cancellationToken)
        {
            var hospitals = await _hospitalRepository.GetAll(
                currentPage: request.PaginationFilter.CurrentPage,
                pageSize: request.PaginationFilter.PageSize,
                orderByField: request.PaginationFilter.OrderBy,
                sortDirection: request.PaginationFilter.Direction,
                queryValue: request.QueryValue,
                cancellationToken);

            return hospitals;
        }
    }
}