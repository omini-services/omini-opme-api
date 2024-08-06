using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class InsuranceCompanyRepository : RepositoryMasterEntity<InsuranceCompany>, IInsuranceCompanyRepository
{
    public InsuranceCompanyRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<InsuranceCompany> Filter(IQueryable<InsuranceCompany> query, string? queryValue = null)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (queryValue is null) return query;

        queryValue = queryValue.ToLower();

        query = query.Where(x => x.Name.LegalName.ToLower().Contains(queryValue)
                                 || x.Name.TradeName.ToLower().Contains(queryValue)
                                 || x.Code.ToLower().Equals(queryValue));

        return query;
    }
}
