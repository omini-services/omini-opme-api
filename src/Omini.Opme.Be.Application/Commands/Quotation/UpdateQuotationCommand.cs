using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Enums;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdateQuotationCommand : IRequest<Result<Quotation, ValidationException>>
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<UpdateQuotationItemCommand> Items { get; set; }

    public class UpdateQuotationItemCommand
    {
        public int LineId { get; set; }
        public int? LineOrder { get; set; }
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string AnvisaCode { get; set; }
        public DateTime AnvisaDueDate { get; set; }
        public double UnitPrice { get; set; }
        public double ItemTotal { get; set; }
        public double Quantity { get; set; }
    }


    public class UpdateQuotationCommandHandler : IRequestHandler<UpdateQuotationCommand, Result<Quotation, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IQuotationRepository _quotationRepository;

        public UpdateQuotationCommandHandler(IUnitOfWork unitOfWork,
                                             IHospitalRepository hospitalRepository,
                                             IPatientRepository patientRepository,
                                             IInsuranceCompanyRepository insuranceCompanyRepository,
                                             IPhysicianRepository physicianRepository,
                                             IQuotationRepository quotationRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
            _patientRepository = patientRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _physicianRepository = physicianRepository;
            _quotationRepository = quotationRepository;
        }

        public async Task<Result<Quotation, ValidationException>> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var hospital = await _hospitalRepository.GetById(request.HospitalId);
            if (hospital is null)
            {
                validationFailures.Add(new ValidationFailure("Hospital Id", "Invalid Id"));
            }

            var patient = await _patientRepository.GetById(request.PatientId);
            if (patient is null)
            {
                validationFailures.Add(new ValidationFailure("Patient Id", "Invalid Id"));
            }

            var insuranceCompany = await _insuranceCompanyRepository.GetById(request.InsuranceCompanyId);
            if (insuranceCompany is null)
            {
                validationFailures.Add(new ValidationFailure("InsuranceCompany Id", "Invalid Id"));
            }

            var physician = await _physicianRepository.GetById(request.PhysicianId);
            if (physician is null)
            {
                validationFailures.Add(new ValidationFailure("Physician Id", "Invalid Id"));
            }

            if (validationFailures.Any())
            {
                return new ValidationException("Invalid quotation values", validationFailures);
            }

            var quotation = await _quotationRepository.GetById(request.Id);
            if (quotation is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            quotation.Number = request.Number;
            quotation.PatientId = request.PatientId;
            quotation.PhysicianId = request.PhysicianId;
            quotation.PayingSourceType = request.PayingSourceType;
            quotation.PayingSourceId = request.PayingSourceId;
            quotation.HospitalId = request.HospitalId;
            quotation.InsuranceCompanyId = request.InsuranceCompanyId;
            quotation.InternalSpecialistId = request.InternalSpecialistId;
            quotation.DueDate = request.DueDate;
            quotation.Items = request.Items.Select((item, index) => new QuotationItem
            {
                QuotationId = quotation.Id,
                LineId = item.LineId,
                LineOrder = item.LineOrder ?? index,
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                AnvisaCode = item.AnvisaCode,
                AnvisaDueDate = item.AnvisaDueDate,
                UnitPrice = item.UnitPrice,
                ItemTotal = item.ItemTotal,
                Quantity = item.Quantity,
            }).ToList();

            await _unitOfWork.Commit();

            return quotation;
        }
    }
}