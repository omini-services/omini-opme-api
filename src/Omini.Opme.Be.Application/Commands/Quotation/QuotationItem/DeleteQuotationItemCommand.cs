using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record DeleteQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; init; }
    public int LineId { get; set; }

    public class DeleteQuotationItemCommandHandler : ICommandHandler<DeleteQuotationItemCommand, Quotation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotationRepository _quotationRepository;

        public DeleteQuotationItemCommandHandler(IUnitOfWork unitOfWork,
                                                 IQuotationRepository quotationRepository)
        {
            _unitOfWork = unitOfWork;
            _quotationRepository = quotationRepository;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(DeleteQuotationItemCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationRepository.GetById(request.QuotationId, cancellationToken);
            if (quotation is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.QuotationId), "Invalid Id"));
            }

            var quotationItem = quotation.Items.SingleOrDefault(i => i.LineId == request.LineId);
            if (quotationItem is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.QuotationId), "Invalid Id"));
            }          

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            quotation.Items.Remove(quotationItem!);

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}