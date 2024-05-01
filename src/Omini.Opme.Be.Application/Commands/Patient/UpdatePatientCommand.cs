using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdatePatientCommand : IRequest<Result<Patient, ValidationException>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Patient, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        public UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Patient, ValidationException>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetById(request.Id);
            if (patient is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            patient.Cpf = StringFormatter.FormatCpf(request.Cpf);
            patient.Name = new PersonName(request.FirstName, request.MiddleName, request.LastName);
            patient.Comments = request.Comments;

            await _unitOfWork.Commit();

            return patient;
        }
    }
}