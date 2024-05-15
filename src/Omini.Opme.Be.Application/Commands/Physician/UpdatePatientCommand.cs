using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdatePhysicianCommand : ICommand<Physician>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; set; }
    public string? MiddleName { get; init; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }


    public class UpdatePhysicianCommandHandler : ICommandHandler<UpdatePhysicianCommand, Physician>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;
        public UpdatePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository physicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = physicianRepository;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(UpdatePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetById(request.Id, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            physician.Cro = request.Cro;
            physician.Crm = request.Crm;
            physician.Name = new PersonName(request.FirstName, request.LastName, request.MiddleName);
            physician.Comments = request.Comments;

            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}