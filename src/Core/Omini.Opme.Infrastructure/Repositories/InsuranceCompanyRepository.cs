using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class InsuranceCompanyRepository : RepositoryMasterEntity<InsuranceCompany>, IInsuranceCompanyRepository
{
    public InsuranceCompanyRepository(OpmeContext context) : base(context)
    {
    }
}
