using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

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
            var insuranceCompany = new InsuranceCompany()
            {
                Cnpj = request.Cnpj,
                Name = new CompanyName(request.LegalName, request.TradeName),
                Comments = request.Comments
            };

            await _insuranceCompanyRepository.Add(insuranceCompany, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return insuranceCompany;
        }
    }
}