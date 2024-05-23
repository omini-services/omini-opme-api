using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Domain.Services;

public interface IHospitalService
{
    public Task<Hospital> Add(Hospital hospital, CancellationToken cancellationToken = default);
    public Task<Hospital> Update(Hospital hospital, CancellationToken cancellationToken = default);
    public Task<Hospital> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<Hospital> GetById(Guid id, CancellationToken cancellationToken = default);
}