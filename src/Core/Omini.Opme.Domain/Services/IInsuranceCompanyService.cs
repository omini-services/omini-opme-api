using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Domain.Services;

public interface IInsuranceCompanyService
{
    public Task<InsuranceCompany> Add(InsuranceCompany insuranceCompany, CancellationToken cancellationToken = default);
    public Task<InsuranceCompany> Update(InsuranceCompany insuranceCompany, CancellationToken cancellationToken = default);
    public Task<InsuranceCompany> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<InsuranceCompany> GetById(Guid id, CancellationToken cancellationToken = default);
}