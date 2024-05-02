using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteQuotationCommand : IRequest<Result<Quotation, ValidationException>>
{
    public Guid Id { get; init; }

    public class DeleteQuotationCommandHandler : IRequestHandler<DeleteQuotationCommand, Result<Quotation, ValidationException>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IAuditableService _auditableService;

        public DeleteQuotationCommandHandler(IUnitOfWork unitOfWork,
                                        IQuotationRepository hospitalRepository,
                                        IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _quotationRepository = hospitalRepository;
            _auditableService = auditableService;
        }

        public async Task<Result<Quotation, ValidationException>> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = await _quotationRepository.GetById(request.Id);
            if (quotation is null)
            {
                return new ValidationException([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(quotation);

            _quotationRepository.Update(quotation);
            await _unitOfWork.Commit();

            return quotation;
        }
    }
}