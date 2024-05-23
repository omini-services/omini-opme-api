using FluentValidation.Results;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business;

public class InsuranceCompanyService : IInsuranceCompanyService
{
    private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public InsuranceCompanyService(IInsuranceCompanyRepository insuranceCompany, IUnitOfWork unitOfWork)
    {
        _insuranceCompanyRepository = insuranceCompany;
        _unitOfWork = unitOfWork;
    }

    public async Task<InsuranceCompany> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _insuranceCompanyRepository.GetById(id, cancellationToken);
    }

    public async Task<InsuranceCompany> Add(InsuranceCompany insuranceCompany, CancellationToken cancellationToken = default)
    {
        await _insuranceCompanyRepository.Add(insuranceCompany, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return insuranceCompany;
    }

    public async Task<InsuranceCompany> Update(InsuranceCompany insuranceCompany, CancellationToken cancellationToken = default)
    {
        var insuranceCompanyExists = await _insuranceCompanyRepository.GetById(insuranceCompany.Id, cancellationToken);
        if (insuranceCompanyExists is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(insuranceCompany.Id), "Invalid id")]);
        }

        _insuranceCompanyRepository.Update(insuranceCompany, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return insuranceCompany;
    }

    public async Task<InsuranceCompany> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var insuranceCompany = await _insuranceCompanyRepository.GetById(id, cancellationToken);
        if (insuranceCompany is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(insuranceCompany);

        _insuranceCompanyRepository.Delete(id, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return insuranceCompany;
    }
}