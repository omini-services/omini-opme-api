using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeleteHospitalCommand : ICommand<Hospital>
{
    public Guid Id { get; init; }

    public class DeleteHospitalCommandHandler : ICommandHandler<DeleteHospitalCommand, Hospital>
    {
        private readonly IHospitalService _hospitalService;

        public DeleteHospitalCommandHandler(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalService.GetById(request.Id, cancellationToken);
            if (hospital is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _hospitalService.Delete(hospital.Id, cancellationToken);

            return hospital;
        }
    }
}