using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreateQuotationCommand : ICommand<Quotation>
{
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<CreateQuotationItemCommand> Items { get; set; }

    public class CreateQuotationItemCommand
    {
        public int? LineOrder { get; set; }
        public string ItemCode { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
    }

    public class CreateQuotationCommandHandler : ICommandHandler<CreateQuotationCommand, Quotation>
    {
        private readonly IHospitalService _hospitalService;
        private readonly IPatientService _patientService;
        private readonly IInsuranceCompanyService _insuranceCompanyService;
        private readonly IPhysicianService _physicianService;
        private readonly IItemService _itemService;
        private readonly IQuotationService _quotationService;

        public CreateQuotationCommandHandler(IHospitalService hospitalService,
                                             IPatientService patientService,
                                             IInsuranceCompanyService insuranceCompanyService,
                                             IPhysicianService physicianService,
                                             IItemService itemService,
                                             IQuotationService quotationService)
        {
            _hospitalService = hospitalService;
            _patientService = patientService;
            _insuranceCompanyService = insuranceCompanyService;
            _physicianService = physicianService;
            _itemService = itemService;
            _quotationService = quotationService;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var hospital = await _hospitalService.GetById(request.HospitalId, cancellationToken);
            if (hospital is null)
            {
                validationFailures.Add(new ValidationFailure("Hospital Id", "Invalid Id"));
            }

            var patient = await _patientService.GetById(request.PatientId, cancellationToken);
            if (patient is null)
            {
                validationFailures.Add(new ValidationFailure("Patient Id", "Invalid Id"));
            }

            var insuranceCompany = await _insuranceCompanyService.GetById(request.InsuranceCompanyId, cancellationToken);
            if (insuranceCompany is null)
            {
                validationFailures.Add(new ValidationFailure("InsuranceCompany Id", "Invalid Id"));
            }

            var physician = await _physicianService.GetById(request.PhysicianId, cancellationToken);
            if (physician is null)
            {
                validationFailures.Add(new ValidationFailure("Physician Id", "Invalid Id"));
            }

            var items = await _itemService.Get(p => request.Items.Select(x => x.ItemCode).Contains(p.Code));

            var quotation = new Quotation()
            {
                Number = request.Number,
                PatientId = request.PatientId,
                PhysicianId = request.PhysicianId,
                PayingSourceType = request.PayingSourceType,
                PayingSourceId = request.PayingSourceId,
                HospitalId = request.HospitalId,
                InsuranceCompanyId = request.InsuranceCompanyId,
                InternalSpecialistId = request.InternalSpecialistId,
                DueDate = request.DueDate.ToUniversalTime(),
            };

            foreach (var (requestItem, index) in request.Items.Select((i, index) => (i, index)))
            {
                var item = items.SingleOrDefault(p => p.Code == requestItem.ItemCode);
                if (item is null)
                {
                    validationFailures.Add(new ValidationFailure("Item ItemCode", $"Invalid ItemCode {requestItem.ItemCode}"));
                    continue;
                }

                quotation.AddItem(new QuotationItem
                {
                    LineId = index,
                    LineOrder = requestItem.LineOrder ?? index,
                    ItemId = item.Id,
                    ItemCode = requestItem.ItemCode,
                    ItemName = item.Name,
                    ReferenceCode = "ref",
                    AnvisaCode = item.AnvisaCode ?? string.Empty,
                    AnvisaDueDate = item.AnvisaDueDate?.ToUniversalTime() ?? DateTime.Now.ToUniversalTime(),
                    UnitPrice = requestItem.UnitPrice,
                    Quantity = requestItem.Quantity,
                });
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            await _quotationService.Add(quotation, cancellationToken);

            return quotation;
        }
    }
}