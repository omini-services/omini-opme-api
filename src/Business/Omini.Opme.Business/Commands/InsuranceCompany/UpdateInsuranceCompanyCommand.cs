using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateInsuranceCompanyCommand : ICommand<InsuranceCompany>
{
    public Guid Id { get; init; }
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateInsuranceCompanyCommandHandler : ICommandHandler<UpdateInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService;
        public UpdateInsuranceCompanyCommandHandler(IInsuranceCompanyService insuranceCompanyService)
        {
            _insuranceCompanyService = insuranceCompanyService;
        }

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(UpdateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyService.GetById(request.Id, cancellationToken);
            if (insuranceCompany is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            insuranceCompany.WithCnpj(request.Cnpj);
            insuranceCompany.Name = new CompanyName(request.LegalName, request.TradeName);
            insuranceCompany.Comments = request.Comments;

            await _insuranceCompanyService.Update(insuranceCompany, cancellationToken);

            return insuranceCompany;
        }
    }
}