using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteInsuranceCompanyCommand : IRequest<Result<InsuranceCompany, ValidationException>>
{
    public Guid Id { get; init; }

    public class DeleteInsuranceCompanyCommandHandler : IRequestHandler<DeleteInsuranceCompanyCommand, Result<InsuranceCompany, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IAuditableService _auditableService;

        public DeleteInsuranceCompanyCommandHandler(IUnitOfWork unitOfWork,
                                        IInsuranceCompanyRepository insuranceCompanyRepository,
                                        IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _auditableService = auditableService;
        }

        public async Task<Result<InsuranceCompany, ValidationException>> Handle(DeleteInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.Id);
            if (insuranceCompany is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(insuranceCompany);

            _insuranceCompanyRepository.Update(insuranceCompany);
            await _unitOfWork.Commit();

            return insuranceCompany;
        }
    }
}