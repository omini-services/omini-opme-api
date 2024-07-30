using FluentValidation.Results;
using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeleteHospitalCommand : ICommand<Hospital>
{
    public string Code { get; init; }

    public class DeleteHospitalCommandHandler : ICommandHandler<DeleteHospitalCommand, Hospital>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;

        public DeleteHospitalCommandHandler(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetByCode(request.Code, cancellationToken);
            if (hospital is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            _hospitalRepository.Delete(hospital, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return hospital;
        }
    }
}