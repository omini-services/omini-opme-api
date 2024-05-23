using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeletePatientCommand : ICommand<Patient>
{
    public Guid Id { get; init; }

    public class DeletePatientCommandHandler : ICommandHandler<DeletePatientCommand, Patient>
    {
        private readonly IPatientService _patientService;

        public DeletePatientCommandHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetById(request.Id, cancellationToken);
            if (patient is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _patientService.Delete(patient.Id, cancellationToken);

            return patient;
        }
    }
}