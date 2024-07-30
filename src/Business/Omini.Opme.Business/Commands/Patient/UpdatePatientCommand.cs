using FluentValidation.Results;
using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdatePatientCommand : ICommand<Patient>
{
    public string Code { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class UpdatePatientCommandHandler : ICommandHandler<UpdatePatientCommand, Patient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        public UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Patient, ValidationResult>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByCode(request.Code, cancellationToken);
            if (patient is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            patient.SetData(
                name: new PersonName(request.FirstName, request.LastName, request.MiddleName),
                cpf: request.Cpf,
                comments: request.Comments);

            _patientRepository.Update(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return patient;
        }
    }
}