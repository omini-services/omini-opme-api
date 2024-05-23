using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public sealed record CreateHospitalCommand : ICommand<Hospital>
{
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    internal sealed class CreateHospitalCommandHandler : ICommandHandler<CreateHospitalCommand, Hospital>
    {
        private readonly IHospitalService _hospitalService;
        public CreateHospitalCommandHandler(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = new Hospital()
            {
                Name = new CompanyName(request.LegalName, request.TradeName),
                Comments = request.Comments
            }.WithCnpj(request.Cnpj);

            await _hospitalService.Add(hospital, cancellationToken);

            return hospital;
        }
    }
}