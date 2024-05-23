using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeleteInsuranceCompanyCommand : ICommand<InsuranceCompany>
{
    public Guid Id { get; init; }

    public class DeleteInsuranceCompanyCommandHandler : ICommandHandler<DeleteInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService;

        public DeleteInsuranceCompanyCommandHandler(IInsuranceCompanyService insuranceCompanyService)
        {
            _insuranceCompanyService = insuranceCompanyService;
        }

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(DeleteInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyService.GetById(request.Id, cancellationToken);
            if (insuranceCompany is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _insuranceCompanyService.Delete(insuranceCompany.Id, cancellationToken);

            return insuranceCompany;
        }
    }
}