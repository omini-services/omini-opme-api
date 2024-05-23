using FluentValidation.Results;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Domain.Services;

public interface IPatientService
{
    public Task<Patient> Add(Patient patient, CancellationToken cancellationToken = default);
    public Task<Patient> Update(Patient patient, CancellationToken cancellationToken = default);
    public Task<Patient> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<Patient> GetById(Guid id, CancellationToken cancellationToken = default);
}