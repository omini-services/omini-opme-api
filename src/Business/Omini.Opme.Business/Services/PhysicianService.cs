using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business;

public class PhysicianService : IPhysicianService
{
    private readonly IPhysicianRepository _physicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PhysicianService(IPhysicianRepository physicianRepository, IUnitOfWork unitOfWork)
    {
        _physicianRepository = physicianRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Physician> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _physicianRepository.GetById(id, cancellationToken);
    }

    public async Task<Physician> Add(Physician physician, CancellationToken cancellationToken = default)
    {
        await _physicianRepository.Add(physician, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return physician;
    }

    public async Task<Physician> Update(Physician physician, CancellationToken cancellationToken = default)
    {
        var physicianExists = await _physicianRepository.GetById(physician.Id, cancellationToken);
        if (physicianExists is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(physician.Id), "Invalid id")]);
        }

        _physicianRepository.Update(physician, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return physician;
    }

    public async Task<Physician> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var physician = await _physicianRepository.GetById(id, cancellationToken);
        if (physician is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(Physician);

        _physicianRepository.Delete(id, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return physician;
    }
}