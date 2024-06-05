using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateHospitalCommand : ICommand<Hospital>
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateHospitalCommandHandler : ICommandHandler<UpdateHospitalCommand, Hospital>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        public UpdateHospitalCommandHandler(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetByCode(request.Code, cancellationToken);
            if (hospital is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            hospital.SetData(
                code: request.Code,
                name: new CompanyName(request.LegalName, request.TradeName),
                cnpj: request.Cnpj,
                comments: request.Comments);

            _hospitalRepository.Update(hospital);
            await _unitOfWork.Commit(cancellationToken);

            return hospital;
        }
    }
}