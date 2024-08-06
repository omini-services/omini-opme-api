using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Queries;

public class GetAllInsuranceCompaniesQuery : IQuery<InsuranceCompany>
{
    public GetAllInsuranceCompaniesQuery() { }

    public GetAllInsuranceCompaniesQuery(string queryValue, PaginationFilter paginationFilter)
    {
        QueryValue = queryValue;
        PaginationFilter = paginationFilter;
    }

    public string QueryValue { get; set; }
    public PaginationFilter PaginationFilter { get; set; }


    public class GetAllInsuranceCompaniesQueryHandler : IQueryHandler<GetAllInsuranceCompaniesQuery, InsuranceCompany>
    {
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        public GetAllInsuranceCompaniesQueryHandler(IInsuranceCompanyRepository insuranceCompanyRepository)
        {
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<PagedResult<InsuranceCompany>> Handle(GetAllInsuranceCompaniesQuery request, CancellationToken cancellationToken)
        {
            var insuranceCompanies = await _insuranceCompanyRepository.GetAll(
                currentPage: request.PaginationFilter.CurrentPage,
                pageSize: request.PaginationFilter.PageSize,
                orderByField: request.PaginationFilter.OrderBy,
                sortDirection: request.PaginationFilter.Direction,
                queryValue: request.QueryValue,
                cancellationToken);

            return insuranceCompanies;
        }
    }
}