using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreatePatientCommand : ICommand<Patient>
{
    public string Code { get; set; }
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
            var patient = new Patient(
                code: request.Code,
                name: new PersonName(request.FirstName, request.LastName, request.MiddleName),
                cpf: request.Cpf,
                comments: request.Comments
            );

            await _patientRepository.Add(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return patient;
        }
    }
}