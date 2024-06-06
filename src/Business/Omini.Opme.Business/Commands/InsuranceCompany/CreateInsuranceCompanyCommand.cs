using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        public CreateInsuranceCompanyCommandHandler(IUnitOfWork unitOfWork, IInsuranceCompanyRepository insuranceCompanyRepository)
        {
            _unitOfWork = unitOfWork;
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(CreateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = new InsuranceCompany(
                name: new CompanyName(request.LegalName, request.TradeName),
                cnpj: request.Cnpj,
                comments: request.Comments
            );

            await _insuranceCompanyRepository.Add(insuranceCompany, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return insuranceCompany;
        }
    }
}