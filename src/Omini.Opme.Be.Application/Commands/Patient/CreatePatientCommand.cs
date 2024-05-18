using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreatePatientCommand : ICommand<Patient>
{
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, Patient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        public CreatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient()
            {
                Cpf = Formatters.FormatCpf(request.Cpf),
                Name = new PersonName(request.FirstName, request.LastName, request.MiddleName),
                Comments = request.Comments
            };

            await _patientRepository.Add(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return patient;
        }
    }
}