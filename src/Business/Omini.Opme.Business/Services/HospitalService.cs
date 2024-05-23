using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business;

public class HospitalService : IHospitalService
{
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IUnitOfWork _unitOfWork;
    public HospitalService(IHospitalRepository hospitalRepository, IUnitOfWork unitOfWork)
    {
        _hospitalRepository = hospitalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Hospital> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _hospitalRepository.GetById(id, cancellationToken);
    }

    public async Task<Hospital> Add(Hospital hospital, CancellationToken cancellationToken = default)
    {
        await _hospitalRepository.Add(hospital, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return hospital;
    }

    public async Task<Hospital> Update(Hospital hospital, CancellationToken cancellationToken = default)
    {
        var hospitalExists = await _hospitalRepository.GetById(hospital.Id, cancellationToken);
        if (hospitalExists is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(hospital.Id), "Invalid id")]);
        }

        _hospitalRepository.Update(hospital, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return hospital;
    }

    public async Task<Hospital> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var hospital = await _hospitalRepository.GetById(id, cancellationToken);
        if (hospital is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(hospital);

        _hospitalRepository.Delete(id, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return hospital;
    }
}