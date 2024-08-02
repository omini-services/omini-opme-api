using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal sealed class InsuranceCompanyRepository : RepositoryMasterEntity<InsuranceCompany>, IInsuranceCompanyRepository
{
    public InsuranceCompanyRepository(OpmeContext context) : base(context)
    {
    }

    public override IQueryable<InsuranceCompany> Filter(IQueryable<InsuranceCompany> query, string? queryField, string? queryValue)
    {
        if (queryField is not null && queryValue is not null)
        {
            queryValue = queryValue.ToLower();
            switch (queryField.ToLowerInvariant())
            {
                case var code when code == nameof(InsuranceCompany.Code).ToLower():
                    query = query.Where(x => x.Code == queryValue);
                    break;

                case var name when name == nameof(InsuranceCompany.Name.TradeName).ToLowerInvariant() || name == nameof(InsuranceCompany.Name.LegalName).ToLowerInvariant():
                    query = query.Where(x => x.Name.LegalName.ToLower().Contains(queryValue)
                                             || x.Name.TradeName.ToLower().Contains(queryValue));
                    break;

                default:
                    return query;
            }
        }

        return query;
    }
}
