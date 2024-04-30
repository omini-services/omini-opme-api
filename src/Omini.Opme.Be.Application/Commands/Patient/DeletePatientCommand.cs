using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeletePatientCommand : IRequest<Result<Patient, ValidationException>>
{
    public Guid Id { get; init; }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<Patient, ValidationException>>
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

        public async Task<Result<Patient, ValidationException>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetById(request.Id);
            if (patient is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(patient);

            _patientRepository.Update(patient);
            await _unitOfWork.Commit();

            return patient;
        }
    }
}