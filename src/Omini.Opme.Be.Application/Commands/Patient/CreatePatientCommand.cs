using FluentValidation;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreatePatientCommand : IRequest<Result<Patient, ValidationException>>
{
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Patient, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        public CreatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Patient, ValidationException>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient(){
                Cpf = request.Cpf,
                Name = new PersonName(request.FirstName, request.MiddleName, request.LastName),
                Comments = request.Comments
            };

            await _patientRepository.Add(patient);
            await _unitOfWork.Commit();

            return patient;
        }
    }
}