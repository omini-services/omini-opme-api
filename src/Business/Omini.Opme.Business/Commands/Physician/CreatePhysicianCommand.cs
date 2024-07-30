using FluentValidation.Results;
using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreatePhysicianCommand : ICommand<Physician>
{
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }

    public class CreatePhysicianCommandHandler : ICommandHandler<CreatePhysicianCommand, Physician>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianServicIPhysicianRepository;
        public CreatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianServicIPhysicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianServicIPhysicianRepository = physicianServicIPhysicianRepository;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(CreatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = new Physician(
                name: new PersonName(request.FirstName, request.LastName, request.MiddleName),
                cro: request.Cro,
                crm: request.Crm,
                comments: request.Comments
            );

            await _physicianServicIPhysicianRepository.Add(physician, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}