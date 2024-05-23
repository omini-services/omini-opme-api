using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateHospitalCommand : ICommand<Hospital>
{
    public Guid Id { get; init; }
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateHospitalCommandHandler : ICommandHandler<UpdateHospitalCommand, Hospital>
    {
        private readonly IHospitalService _hospitalService;
        public UpdateHospitalCommandHandler(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalService.GetById(request.Id, cancellationToken);
            if (hospital is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            hospital.WithCnpj(request.Cnpj);
            hospital.Name = new CompanyName(request.LegalName, request.TradeName);
            hospital.Comments = request.Comments;

            await _hospitalService.Update(hospital);

            return hospital;
        }
    }
}