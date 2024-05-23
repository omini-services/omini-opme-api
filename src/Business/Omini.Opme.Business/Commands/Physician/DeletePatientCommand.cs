using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeletePhysicianCommand : ICommand<Physician>
{
    public Guid Id { get; init; }

    public class DeletePhysicianCommandHandler : ICommandHandler<DeletePhysicianCommand, Physician>
    {
        private readonly IPhysicianService _physicianService;

        public DeletePhysicianCommandHandler(IPhysicianService PhysicianService )
        {
            _physicianService = PhysicianService;
        }

        public async Task<Result<Physician, ValidationResult>> Handle(DeletePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianService.GetById(request.Id, cancellationToken);
            if (physician is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _physicianService.Delete(physician.Id, cancellationToken);

            return physician;
        }
    }
}