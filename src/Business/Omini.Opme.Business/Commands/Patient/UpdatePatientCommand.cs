using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdatePatientCommand : ICommand<Patient>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class UpdatePatientCommandHandler : ICommandHandler<UpdatePatientCommand, Patient>
    {
        private readonly IPatientService _patientService;
        public UpdatePatientCommandHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetById(request.Id, cancellationToken);
            if (patient is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            patient.WithCpf(request.Cpf);
            patient.Name = new PersonName(request.FirstName, request.LastName, request.MiddleName);
            patient.Comments = request.Comments;

            await _patientService.Update(patient, cancellationToken);

            return patient;
        }
    }
}