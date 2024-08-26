using FluentValidation.Results;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Business.Abstractions.Messaging;

namespace Omini.Opme.Business.Commands;

public record UpdateQuotationCommand : ICommand<Quotation>
{
    public Guid Id { get; init; }
    public string PatientCode { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientMiddleName { get; set; }
    public string PatientLastName { get; set; }
    public string PhysicianCode { get; set; }
    public string PhysicianFirstName { get; set; }
    public string PhysicianMiddleName { get; set; }
    public string PhysicianLastName { get; set; }
    public string HospitalCode { get; set; }
    public string HospitalName { get; set; }
    public string InsuranceCompanyCode { get; set; }
    public string InsuranceCompanyName { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public DateTime DueDate { get; set; }
    public string Comments { get; set; }

    public class UpdateQuotationCommandHandler : ICommandHandler<UpdateQuotationCommand, Quotation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IQuotationRepository _quotationRepository;

        public UpdateQuotationCommandHandler(IUnitOfWork unitOfWork,
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

        public async Task<Result<Quotation, ValidationResult>> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = await _quotationRepository.GetById(request.Id, cancellationToken);
            if (quotation is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

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
            if (insuranceCompany is null && request.PayingSourceType == PayingSourceType.Insurance)
            {
                validationFailures.Add(new ValidationFailure("InsuranceCompany Code", "Invalid code"));
            }

            var physician = await _physicianRepository.GetByCode(request.PhysicianCode);
            if (physician is null)
            {
                validationFailures.Add(new ValidationFailure("Physician Code", "Invalid code"));
            }

            var patientName = new PersonName(firstName: request.PatientFirstName, lastName: request.PatientLastName, middleName: request.PatientMiddleName);
            var physicianName = new PersonName(firstName: request.PhysicianFirstName, lastName: request.PhysicianLastName, middleName: request.PhysicianMiddleName);

            quotation.SetData(
                patientCode: request.PatientCode, patientName: patientName,
                physicianCode: request.PhysicianCode, physicianName: physicianName,
                hospitalCode: hospital.Code, hospitalName: request.HospitalName,
                insuranceCompanyCode: insuranceCompany?.Code!, insuranceCompanyName: request.InsuranceCompanyName,
                internalSpecialistCode: "1",
                payingSourceType: request.PayingSourceType,
                dueDate: request.DueDate,
                comments: request.Comments
            );

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}