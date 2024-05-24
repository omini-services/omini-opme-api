using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateInsuranceCompanyCommand : ICommand<InsuranceCompany>
{
    public Guid Id { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateInsuranceCompanyCommandHandler : ICommandHandler<UpdateInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        public UpdateInsuranceCompanyCommandHandler(IUnitOfWork unitOfWork, IInsuranceCompanyRepository insuranceCompanyRepository)
        {
            _unitOfWork = unitOfWork;
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(UpdateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.Id, cancellationToken);
            if (insuranceCompany is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            insuranceCompany.SetData(new CompanyName(request.LegalName, request.TradeName),
                cnpj: request.Cnpj,
                comments: request.Comments
            );

            _insuranceCompanyRepository.Update(insuranceCompany, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return insuranceCompany;
        }
    }
}