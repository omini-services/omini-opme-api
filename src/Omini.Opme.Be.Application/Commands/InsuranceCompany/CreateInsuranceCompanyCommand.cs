using FluentValidation;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreateInsuranceCompanyCommand : IRequest<Result<InsuranceCompany, ValidationException>>
{
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class CreateInsuranceCompanyCommandHandler : IRequestHandler<CreateInsuranceCompanyCommand, Result<InsuranceCompany, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        public CreateInsuranceCompanyCommandHandler(IUnitOfWork unitOfWork, IInsuranceCompanyRepository insuranceCompanyRepository)
        {
            _unitOfWork = unitOfWork;
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<Result<InsuranceCompany, ValidationException>> Handle(CreateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = new InsuranceCompany()
            {
                Cnpj = request.Cnpj,
                Name = new CompanyName(request.LegalName, request.TradeName),
                Comments = request.Comments
            };

            await _insuranceCompanyRepository.Add(insuranceCompany);
            await _unitOfWork.Commit();

            return insuranceCompany;
        }
    }
}