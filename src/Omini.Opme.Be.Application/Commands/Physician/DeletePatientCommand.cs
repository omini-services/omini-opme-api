using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeletePhysicianCommand : ICommand<Physician>
{
    public Guid Id { get; init; }

    public class DeletePhysicianCommandHandler : ICommandHandler<DeletePhysicianCommand, Physician>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IAuditableService _auditableService;

        public DeletePhysicianCommandHandler(IUnitOfWork unitOfWork,
                                        IPhysicianRepository PhysicianRepository,
                                        IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = PhysicianRepository;
            _auditableService = auditableService;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(DeletePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetById(request.Id, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(physician);

            _physicianRepository.Update(physician, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}