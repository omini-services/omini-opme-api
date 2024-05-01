using FluentValidation;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreateHospitalCommand : IRequest<Result<Hospital, ValidationException>>
{
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class CreateHospitalCommandHandler : IRequestHandler<CreateHospitalCommand, Result<Hospital, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        public CreateHospitalCommandHandler(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result<Hospital, ValidationException>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = new Hospital(){
                Cnpj = request.Cnpj,
                Name = new CompanyName(request.LegalName, request.TradeName),
                Comments = request.Comments
            };

            await _hospitalRepository.Add(hospital);
            await _unitOfWork.Commit();

            return hospital;
        }
    }
}