using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

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
        private readonly IPhysicianRepository _physicianRepository;
        public CreatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = physicianRepository;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(CreatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = new Physician()
            {
                Cro = request.Cro,
                Crm = request.Crm,
                Name = new PersonName(request.FirstName, request.LastName, request.MiddleName),
                Comments = request.Comments
            };

            await _physicianRepository.Add(physician, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}