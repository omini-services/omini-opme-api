

using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure.Repositories;

internal class InsuranceCompanyRepository : Repository<InsuranceCompany>, IInsuranceCompanyRepository
{
    public InsuranceCompanyRepository(OpmeContext context) : base(context)
    {
    }
}
