using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdateInsuranceCompanyCommand : IRequest<Result<InsuranceCompany, ValidationException>>
{
    public Guid Id { get; init; }
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateInsuranceCompanyCommandHandler : IRequestHandler<UpdateInsuranceCompanyCommand, Result<InsuranceCompany, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        public UpdateInsuranceCompanyCommandHandler(IUnitOfWork unitOfWork, IInsuranceCompanyRepository insuranceCompanyRepository)
        {
            _unitOfWork = unitOfWork;
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<Result<InsuranceCompany, ValidationException>> Handle(UpdateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.Id);
            if (insuranceCompany is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            insuranceCompany.Cnpj = request.Cnpj;
            insuranceCompany.Name = new CompanyName(request.LegalName, request.TradeName);
            insuranceCompany.Comments = request.Comments;

            await _unitOfWork.Commit();

            return insuranceCompany;
        }
    }
}