using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreateInsuranceCompanyCommand : ICommand<InsuranceCompany>
{
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class CreateInsuranceCompanyCommandHandler : ICommandHandler<CreateInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService;
        public CreateInsuranceCompanyCommandHandler(IInsuranceCompanyService insuranceCompanyService)
        {
            _insuranceCompanyService = insuranceCompanyService;
        }

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(CreateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = new InsuranceCompany()
            {
                Name = new CompanyName(request.LegalName, request.TradeName),
                Comments = request.Comments
            }.WithCnpj(request.Cnpj);

            await _insuranceCompanyService.Add(insuranceCompany, cancellationToken);

            return insuranceCompany;
        }
    }
}