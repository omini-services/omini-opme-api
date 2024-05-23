using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeleteQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; init; }
    public int LineId { get; set; }

    public class DeleteQuotationItemCommandHandler : ICommandHandler<DeleteQuotationItemCommand, Quotation>
    {
        private readonly IQuotationService _quotationService;

        public DeleteQuotationItemCommandHandler(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(DeleteQuotationItemCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationService.GetById(request.QuotationId, cancellationToken);
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

            quotation.RemoveItem(quotationItem!);

            await _quotationService.Update(quotation, cancellationToken);

            return quotation;
        }
    }
}