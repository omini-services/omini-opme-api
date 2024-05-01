using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeletePhysicianCommand : IRequest<Result<Physician, ValidationException>>
{
    public Guid Id { get; init; }

    public class DeletePhysicianCommandHandler : IRequestHandler<DeletePhysicianCommand, Result<Physician, ValidationException>>
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

        public async Task<Result<Physician, ValidationException>> Handle(DeletePhysicianCommand request, CancellationToken cancellationToken)
        {
            var physician = await _physicianRepository.GetById(request.Id);
            if (physician is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(physician);

            _physicianRepository.Update(physician);
            await _unitOfWork.Commit();

            return physician;
        }
    }
}