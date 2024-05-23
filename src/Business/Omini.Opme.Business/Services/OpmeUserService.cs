// using FluentValidation.Results;
// using Omini.Opme.Be.Domain.Transactions;
// using Omini.Opme.Domain.BusinessPartners;
// using Omini.Opme.Shared.Entities;

// namespace Omini.Opme.Business;

// public class OpmeUserService : IOpmeUserService
// {
//     private readonly IHospitalRepository _hospitalRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     public OpmeUserService(IHospitalRepository hospitalRepository, IUnitOfWork unitOfWork)
//     {
//         _hospitalRepository = hospitalRepository;
//         _unitOfWork = unitOfWork;
//     }

//     public async Task<Result<Hospital, ValidationResult>> Add(Hospital hospital, CancellationToken cancellationToken = default)
//     {
//         await _hospitalRepository.Add(hospital, cancellationToken);
//         await _unitOfWork.Commit(cancellationToken);

//         return hospital;
//     }

//     public async Task<Result<Hospital, ValidationResult>> Update(Guid id, Hospital hospital, CancellationToken cancellationToken = default)
//     {
//         var hospitalExists = await _hospitalRepository.GetById(hospital.Id, cancellationToken);
//         if (hospitalExists is null)
//         {
//             return new ValidationResult([new ValidationFailure(nameof(hospital.Id), "Invalid id")]);
//         }

//         _hospitalRepository.Update(hospital, cancellationToken);

//         await _unitOfWork.Commit(cancellationToken);

//         return hospital;
//     }

//     public async Task<Result<Hospital, ValidationResult>> Delete(Guid id, CancellationToken cancellationToken = default)
//     {
//         var hospital = await _hospitalRepository.GetById(id, cancellationToken);
//         if (hospital is null)
//         {
//             return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
//         }

//         //_auditableService.SoftDelete(hospital);

//         _hospitalRepository.Delete(id, cancellationToken);
//         await _unitOfWork.Commit(cancellationToken);

//         return hospital;
//     }
// }