using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PatientService(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Patient> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _patientRepository.GetById(id, cancellationToken);
    }


    public async Task<Patient> Add(Patient patient, CancellationToken cancellationToken = default)
    {
        await _patientRepository.Add(patient, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return patient;
    }

    public async Task<Patient> Update(Patient patient, CancellationToken cancellationToken = default)
    {
        var patientExists = await _patientRepository.GetById(patient.Id, cancellationToken);
        if (patientExists is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(patient.Id), "Invalid id")]);
        }

        _patientRepository.Update(patient, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return patient;
    }

    public async Task<Patient> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetById(id, cancellationToken);
        if (patient is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(Patient);

        _patientRepository.Delete(id, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return patient;
    }
}