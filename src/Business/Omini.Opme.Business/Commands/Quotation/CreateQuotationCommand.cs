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
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
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
            var hospital = await _hospitalRepository.GetById(request.HospitalId, cancellationToken);
            if (hospital is null)
            {
                validationFailures.Add(new ValidationFailure("Hospital Id", "Invalid Id"));
            }

            var patient = await _patientRepository.GetById(request.PatientId, cancellationToken);
            if (patient is null)
            {
                validationFailures.Add(new ValidationFailure("Patient Id", "Invalid Id"));
            }

            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.InsuranceCompanyId, cancellationToken);
            if (insuranceCompany is null)
            {
                validationFailures.Add(new ValidationFailure("InsuranceCompany Id", "Invalid Id"));
            }

            var physician = await _physicianRepository.GetById(request.PhysicianId, cancellationToken);
            if (physician is null)
            {
                validationFailures.Add(new ValidationFailure("Physician Id", "Invalid Id"));
            }

            var items = await _itemRepository.Get(p => request.Items.Select(x => x.ItemCode).Contains(p.Code));

            var quotation = new Quotation(
                patientId: request.PatientId,
                physicianId: request.PhysicianId,
                payingSourceType: request.PayingSourceType,
                payingSourceId: request.PayingSourceId,
                hospitalId: request.HospitalId,
                insuranceCompanyId: request.InsuranceCompanyId,
                internalSpecialistId: request.InternalSpecialistId,
                dueDate: request.DueDate
            );

            foreach (var (requestItem, index) in request.Items.Select((i, index) => (i, index)))
            {
                var item = items.SingleOrDefault(p => p.Code == requestItem.ItemCode);
                if (item is null)
                {
                    validationFailures.Add(new ValidationFailure("Item ItemCode", $"Invalid ItemCode {requestItem.ItemCode}"));
                    continue;
                }

                quotation.AddItem(
                    itemId: item.Id,
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