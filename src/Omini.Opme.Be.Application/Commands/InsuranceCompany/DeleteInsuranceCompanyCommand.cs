using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteInsuranceCompanyCommand : ICommand<InsuranceCompany>
{
    public Guid Id { get; init; }

    public class DeleteInsuranceCompanyCommandHandler : ICommandHandler<DeleteInsuranceCompanyCommand, InsuranceCompany>
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

        public async Task<Result<InsuranceCompany, ValidationResult>> Handle(DeleteInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.Id, cancellationToken);
            if (insuranceCompany is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(insuranceCompany);

            _insuranceCompanyRepository.Update(insuranceCompany, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return insuranceCompany;
        }
    }
}