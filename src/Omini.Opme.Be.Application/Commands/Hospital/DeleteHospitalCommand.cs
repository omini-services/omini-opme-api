using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteHospitalCommand : IRequest<Result<Hospital, ValidationException>>
{
    public Guid Id { get; init; }

    public class DeleteHospitalCommandHandler : IRequestHandler<DeleteHospitalCommand, Result<Hospital, ValidationException>>
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

        public async Task<Result<Hospital, ValidationException>> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetById(request.Id);
            if (hospital is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(hospital);

            _hospitalRepository.Update(hospital);
            await _unitOfWork.Commit();

            return hospital;
        }
    }
}