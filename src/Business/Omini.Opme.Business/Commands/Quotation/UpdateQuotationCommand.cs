// using FluentValidation.Results;
// using Omini.Opme.Be.Application.Abstractions.Messaging;
// using Omini.Opme.Be.Domain.Entities;
// using Omini.Opme.Be.Domain.Enums;
// using Omini.Opme.Be.Domain.Repositories;
// using Omini.Opme.Be.Domain.Transactions;
// using Omini.Opme.Be.Shared.Entities;

// namespace Omini.Opme.Be.Application.Commands;

// public record UpdateQuotationCommand : ICommand<Quotation>
// {
//     public Guid Id { get; set; }
//     public string Number { get; set; }
//     public Guid PatientId { get; set; }
//     public Guid PhysicianId { get; set; }
//     public PayingSourceType PayingSourceType { get; set; }
//     public Guid PayingSourceId { get; set; }
//     public Guid HospitalId { get; set; }
//     public Guid InsuranceCompanyId { get; set; }
//     public Guid InternalSpecialistId { get; set; }
//     public DateTime DueDate { get; set; }
//     public IEnumerable<UpdateQuotationItemCommand> Items { get; set; }

//     public class UpdateQuotationItemCommand
//     {
//         public int LineId { get; set; }
//         public int? LineOrder { get; set; }
//         public string ItemCode { get; set; }
//         public double UnitPrice { get; set; }
//         public double Quantity { get; set; }
//     }


//     public class UpdateQuotationCommandHandler : ICommandHandler<UpdateQuotationCommand, Quotation>
//     {
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IHospitalRepository _hospitalRepository;
//         private readonly IPatientRepository _patientRepository;
//         private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
//         private readonly IPhysicianRepository _physicianRepository;
//         private readonly IQuotationRepository _quotationRepository;

//         public UpdateQuotationCommandHandler(IUnitOfWork unitOfWork,
//                                              IHospitalRepository hospitalRepository,
//                                              IPatientRepository patientRepository,
//                                              IInsuranceCompanyRepository insuranceCompanyRepository,
//                                              IPhysicianRepository physicianRepository,
//                                              IQuotationRepository quotationRepository)
//         {
//             _unitOfWork = unitOfWork;
//             _hospitalRepository = hospitalRepository;
//             _patientRepository = patientRepository;
//             _insuranceCompanyRepository = insuranceCompanyRepository;
//             _physicianRepository = physicianRepository;
//             _quotationRepository = quotationRepository;
//         }

//         public async Task<Result<Quotation, ValidationResult>> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
//         {
//             var validationFailures = new List<ValidationFailure>();
//             var hospital = await _hospitalRepository.GetById(request.HospitalId, cancellationToken);
//             if (hospital is null)
//             {
//                 validationFailures.Add(new ValidationFailure("Hospital Id", "Invalid Id"));
//             }

//             var patient = await _patientRepository.GetById(request.PatientId, cancellationToken);
//             if (patient is null)
//             {
//                 validationFailures.Add(new ValidationFailure("Patient Id", "Invalid Id"));
//             }

//             var insuranceCompany = await _insuranceCompanyRepository.GetById(request.InsuranceCompanyId, cancellationToken);
//             if (insuranceCompany is null)
//             {
//                 validationFailures.Add(new ValidationFailure("InsuranceCompany Id", "Invalid Id"));
//             }

//             var physician = await _physicianRepository.GetById(request.PhysicianId, cancellationToken);
//             if (physician is null)
//             {
//                 validationFailures.Add(new ValidationFailure("Physician Id", "Invalid Id"));
//             }

//             if (validationFailures.Any())
//             {
//                 return new ValidationResult(validationFailures);
//             }

//             var quotation = await _quotationRepository.GetById(request.Id, cancellationToken);
//             if (quotation is null)
//             {
//                 return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
//             }

//             quotation.Number = request.Number;
//             quotation.PatientId = request.PatientId;
//             quotation.PhysicianId = request.PhysicianId;
//             quotation.PayingSourceType = request.PayingSourceType;
//             quotation.PayingSourceId = request.PayingSourceId;
//             quotation.HospitalId = request.HospitalId;
//             quotation.InsuranceCompanyId = request.InsuranceCompanyId;
//             quotation.InternalSpecialistId = request.InternalSpecialistId;
//             quotation.DueDate = request.DueDate.ToUniversalTime();
//             quotation.Items = request.Items.Select((item, index) => new QuotationItem
//             {
//                 QuotationId = quotation.Id,
//                 LineId = item.LineId,
//                 LineOrder = item.LineOrder ?? index,
//                 ItemId = item.ItemId,
//                 ItemCode = item.ItemCode,
//                 AnvisaCode = item.AnvisaCode,
//                 AnvisaDueDate = item.AnvisaDueDate.ToUniversalTime(),
//                 UnitPrice = item.UnitPrice,
//                 Quantity = item.Quantity,
//                 ItemTotal = item.Quantity * item.UnitPrice,
//             }).ToList();

//             quotation.Total = quotation.Items.Sum(p => p.ItemTotal);

//             await _unitOfWork.Commit(cancellationToken);

//             return quotation;
//         }
//     }
// }