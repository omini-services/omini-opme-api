using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business.Commands;

public record CreateQuotationCommand : ICommand<Quotation>
{
    public string PatientCode { get; set; }
    public string PatientName { get; set; }
    public string PhysicianCode { get; set; }
    public string PhysicianName { get; set; }
    public string HospitalCode { get; set; }
    public string HospitalName { get; set; }
    public string InsuranceCompanyCode { get; set; }
    public string InsuranceCompanyName { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public string PayingSourceCode { get; set; }
    public string PayingSourceName { get; set; }
    public DateTime DueDate { get; set; }
    public List<CreateQuotationItems> Items { get; set; } = new();

    public class CreateQuotationItems
    {
        public int? LineOrder { get; set; }
        public string ItemCode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CreateQuotationCommandHandler : ICommandHandler<CreateQuotationCommand, Quotation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IQuotationRepository _quotationRepository;

        public CreateQuotationCommandHandler(IUnitOfWork unitOfWork,
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

        public async Task<Result<Quotation, ValidationResult>> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var hospital = await _hospitalRepository.GetByCode(request.HospitalCode, cancellationToken);
            if (hospital is null)
            {
                validationFailures.Add(new ValidationFailure("Hospital Code", "Invalid code"));
            }

            var patient = await _patientRepository.GetByCode(request.PatientCode, cancellationToken);
            if (patient is null)
            {
                validationFailures.Add(new ValidationFailure("Patient Code", "Invalid code"));
            }

            var insuranceCompany = await _insuranceCompanyRepository.GetByCode(request.InsuranceCompanyCode);
            if (insuranceCompany is null)
            {
                validationFailures.Add(new ValidationFailure("InsuranceCompany Code", "Invalid code"));
            }

            var physician = await _physicianRepository.GetByCode(request.PhysicianCode);
            if (physician is null)
            {
                validationFailures.Add(new ValidationFailure("Physician Code", "Invalid code"));
            }

            var items = await _itemRepository.Get(p => request.Items.Select(x => x.ItemCode).Contains(p.Code));

            var quotation = new Quotation(
                patientCode: patient.Code, patientName: request.PatientName,
                physicianCode: physician.Code, physicianName: request.PhysicianName,
                hospitalCode: hospital.Code, hospitalName: request.HospitalName,
                insuranceCompanyCode: insuranceCompany.Code, insuranceCompanyName: request.InsuranceCompanyName,
                internalSpecialistCode: "", 
                payingSourceType: request.PayingSourceType, payingSourceCode: request.PayingSourceCode, payingSourceName: request.PayingSourceName,                
                dueDate: request.DueDate
            );

            foreach (var (requestItem, index) in request.Items.Select((i, index) => (i, index)))
            {
                var item = items.SingleOrDefault(p => p.Code == requestItem.ItemCode);
                if (item is null)
                {
                    validationFailures.Add(new ValidationFailure("Item ItemCode", $"Invalid itemCode {requestItem.ItemCode}"));
                    continue;
                }

                quotation.AddItem(
                    itemCode: requestItem.ItemCode,
                    itemName: item.Name,
                    referenceCode: "ref",
                    anvisaCode: item.AnvisaCode ?? string.Empty,
                    anvisaDueDate: item.AnvisaDueDate ?? DateTime.Now,
                    unitPrice: requestItem.UnitPrice,
                    quantity: requestItem.Quantity
                );
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            await _quotationRepository.Add(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}