using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure.Repositories;

internal class InsuranceCompanyRepository : Repository<InsuranceCompany>, IInsuranceCompanyRepository
{
    public InsuranceCompanyRepository(OpmeContext context) : base(context)
    {
    }
}
