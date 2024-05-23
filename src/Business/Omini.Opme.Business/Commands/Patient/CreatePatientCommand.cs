using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreatePatientCommand : ICommand<Patient>
{
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, Patient>
    {
        private readonly IPatientService _patientService;
        public CreatePatientCommandHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient()
            {                
                Name = new PersonName(request.FirstName, request.LastName, request.MiddleName),
                Comments = request.Comments
            }.WithCpf(request.Cpf);

            await _patientService.Add(patient, cancellationToken);

            return patient;
        }
    }
}