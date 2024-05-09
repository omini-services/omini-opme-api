using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteHospitalCommand : ICommand<Hospital>
{
    public Guid Id { get; init; }

    public class DeleteHospitalCommandHandler : ICommandHandler<DeleteHospitalCommand, Hospital>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IAuditableService _auditableService;

        public DeleteHospitalCommandHandler(IUnitOfWork unitOfWork,
                                        IHospitalRepository hospitalRepository,
                                        IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
            _auditableService = auditableService;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetById(request.Id, cancellationToken);
            if (hospital is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(hospital);

            _hospitalRepository.Update(hospital, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return hospital;
        }
    }
}