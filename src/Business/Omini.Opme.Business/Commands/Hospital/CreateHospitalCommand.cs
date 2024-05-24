using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public sealed record CreateHospitalCommand : ICommand<Hospital>
{
    public string LegalName { get; set;}
    public string TradeName { get; set;}
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    internal sealed class CreateHospitalCommandHandler : ICommandHandler<CreateHospitalCommand, Hospital>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        public CreateHospitalCommandHandler(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result<Hospital, ValidationResult>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = new Hospital(
                name: new CompanyName(request.LegalName, request.TradeName),
                cnpj: request.Cnpj,
                comments: request.Comments
            );

            await _hospitalRepository.Add(hospital, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return hospital;
        }
    }
}