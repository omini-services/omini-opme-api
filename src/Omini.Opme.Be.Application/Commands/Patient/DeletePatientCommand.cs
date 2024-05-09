using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeletePatientCommand : ICommand<Patient>
{
    public Guid Id { get; init; }

    public class DeletePatientCommandHandler : ICommandHandler<DeletePatientCommand, Patient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        private readonly IAuditableService _auditableService;

        public DeletePatientCommandHandler(IUnitOfWork unitOfWork,
                                        IPatientRepository patientRepository,
                                        IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
            _auditableService = auditableService;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetById(request.Id, cancellationToken);
            if (patient is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(patient);

            _patientRepository.Update(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return patient;
        }
    }
}