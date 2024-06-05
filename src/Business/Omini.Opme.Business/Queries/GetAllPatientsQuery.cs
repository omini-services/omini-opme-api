using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllPatientsQuery : IQuery<Patient>
{
    public GetAllPatientsQuery() { }

    public GetAllPatientsQuery(QueryFilter queryFilter, PaginationFilter paginationFilter)
    {
        QueryFilter = queryFilter;
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public QueryFilter QueryFilter { get; set; }
    public class GetAllPatientsQueryHandler : IQueryHandler<GetAllPatientsQuery, Patient>
    {
        private readonly IPatientRepository _patientRepository;
        public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PagedResult<Patient>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAll(
                currentPage: request.PaginationFilter.CurrentPage,
                pageSize: request.PaginationFilter.PageSize,
                orderByField: request.PaginationFilter.OrderBy,
                sortDirection: request.PaginationFilter.Direction,
                queryField: request.QueryFilter.QueryField,
                queryValue: request.QueryFilter.QueryValue,
                cancellationToken);

            return patients;
        }
    }
}