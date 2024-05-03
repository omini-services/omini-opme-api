using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdateHospitalCommand : IRequest<Result<Hospital, ValidationException>>
{
    public Guid Id { get; init; }
    public string LegalName { get; init; }
    public string TradeName { get; init; }
    public string Cnpj { get; set; }
    public string Comments { get; set; }

    public class UpdateHospitalCommandHandler : IRequestHandler<UpdateHospitalCommand, Result<Hospital, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHospitalRepository _hospitalRepository;
        public UpdateHospitalCommandHandler(IUnitOfWork unitOfWork, IHospitalRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result<Hospital, ValidationException>> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetById(request.Id);
            if (hospital is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            hospital.Cnpj = Formatters.FormatCnpj(request.Cnpj);
            hospital.Name = new CompanyName(request.LegalName, request.TradeName);
            hospital.Comments = request.Comments;

            await _unitOfWork.Commit();

            return hospital;
        }
    }
}