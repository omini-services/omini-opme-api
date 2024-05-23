using FluentValidation.Results;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Domain.Services;

public interface IPhysicianService
{
    public Task<Physician> Add(Physician physician, CancellationToken cancellationToken = default);
    public Task<Physician> Update(Physician physician, CancellationToken cancellationToken = default);
    public Task<Physician> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<Physician> GetById(Guid id, CancellationToken cancellationToken = default);
}