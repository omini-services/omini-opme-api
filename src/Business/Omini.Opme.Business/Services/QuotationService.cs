using FluentValidation.Results;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business;

public class QuotationService : IQuotationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
    private readonly IPhysicianRepository _physicianRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IQuotationRepository _quotationRepository;

    public QuotationService(IUnitOfWork unitOfWork,
                            IHospitalRepository hospitalRepository,
                            IPatientRepository patientRepository,
                            IInsuranceCompanyRepository insuranceCompanyRepository,
                            IPhysicianRepository physicianRepository,
                            IItemRepository itemRepository,
                            IQuotationRepository quotationRepository)
    {
        _unitOfWork = unitOfWork;
        _hospitalRepository = hospitalRepository;
        _patientRepository = patientRepository;
        _insuranceCompanyRepository = insuranceCompanyRepository;
        _physicianRepository = physicianRepository;
        _itemRepository = itemRepository;
        _quotationRepository = quotationRepository;
    }
    public async Task<Quotation> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _quotationRepository.GetById(id, cancellationToken);
    }

    public async Task<Quotation> Add(Quotation quotation, CancellationToken cancellationToken = default)
    {
        // var validationFailures = new List<ValidationFailure>();
        // var hospital = await _hospitalRepository.GetById(quotation.HospitalId, cancellationToken);
        // if (hospital is null)
        // {
        //     //validationFailures.Add(new ValidationFailure("Hospital Id", "Invalid Id"));
        // }

        // var patient = await _patientRepository.GetById(quotation.PatientId, cancellationToken);
        // if (patient is null)
        // {
        //     //validationFailures.Add(new ValidationFailure("Patient Id", "Invalid Id"));
        // }

        // var insuranceCompany = await _insuranceCompanyRepository.GetById(quotation.InsuranceCompanyId, cancellationToken);
        // if (insuranceCompany is null)
        // {
        //     //validationFailures.Add(new ValidationFailure("InsuranceCompany Id", "Invalid Id"));
        // }

        // var physician = await _physicianRepository.GetById(quotation.PhysicianId, cancellationToken);
        // if (physician is null)
        // {
        //     //validationFailures.Add(new ValidationFailure("Physician Id", "Invalid Id"));
        // }

        // var items = await _itemRepository.Get(p => quotation.Items.Select(x => x.ItemCode).Contains(p.Code));

        // foreach (var (quotationItem, index) in quotation.Items.Select((i, index) => (i, index)))
        // {
        //     var item = items.SingleOrDefault(p => p.Code == quotationItem.ItemCode);
        //     if (item is null)
        //     {
        //         validationFailures.Add(new ValidationFailure("Item ItemCode", $"Invalid ItemCode {quotationItem.ItemCode}"));
        //         continue;
        //     }
        // }

        // if (validationFailures.Any())
        // {
        //     //return new ValidationResult(validationFailures);
        // }

        quotation.Total = quotation.Items.Sum(p => p.ItemTotal);

        await _quotationRepository.Add(quotation, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return quotation;
    }

    public async Task<Quotation> Update(Quotation quotation, CancellationToken cancellationToken = default)
    {
        return quotation;
    }

    public async Task<Quotation> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var quotation = await _quotationRepository.GetById(id, cancellationToken);
        if (quotation is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(quotation);

        _quotationRepository.Update(quotation, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return quotation;
    }
}