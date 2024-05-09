using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteQuotationCommand : ICommand<Quotation>
{
    public Guid Id { get; init; }

    public class DeleteQuotationCommandHandler : ICommandHandler<DeleteQuotationCommand, Quotation>
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

        public async Task<Result<Quotation, ValidationResult>> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = await _quotationRepository.GetById(request.Id, cancellationToken);
            if (quotation is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _auditableService.SoftDelete(quotation);

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}