using FluentValidation.Results;
using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeletePhysicianCommand : ICommand<Physician>
{
    public string Code { get; init; }

    public class DeletePhysicianCommandHandler : ICommandHandler<DeletePhysicianCommand, Physician>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhysicianRepository _physicianRepository;

        public DeletePhysicianCommandHandler(IUnitOfWork unitOfWork, IPhysicianRepository PhysicianRepository)
        {
            _unitOfWork = unitOfWork;
            _physicianRepository = PhysicianRepository;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(DeletePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetByCode(request.Code, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            _physicianRepository.Delete(physician, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return physician;
        }
    }
}