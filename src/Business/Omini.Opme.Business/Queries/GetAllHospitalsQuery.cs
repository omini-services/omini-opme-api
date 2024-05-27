using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllHospitalsQuery : IQuery<Hospital>
{
    public GetAllHospitalsQuery() { }

    public GetAllHospitalsQuery(PaginationFilter paginationFilter)
    {
        PaginationFilter = paginationFilter;
    }

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
            var hospitals = await _hospitalRepository.GetAllPaginated(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);            

            return hospitals;
        }
    }
}