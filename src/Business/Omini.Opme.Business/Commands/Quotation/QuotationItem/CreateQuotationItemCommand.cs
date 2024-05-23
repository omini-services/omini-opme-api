using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record CreateQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; set; }
    public int? LineOrder { get; set; }
    public string ItemCode { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }

    public class CreateQuotationItemCommandHandler : ICommandHandler<CreateQuotationItemCommand, Quotation>
    {
        private readonly IItemService _itemService;
        private readonly IQuotationService _quotationService;

        public CreateQuotationItemCommandHandler(IItemService itemService,
                                                 IQuotationService quotationService)
        {
            _itemService = itemService;
            _quotationService = quotationService;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(CreateQuotationItemCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationService.GetById(request.QuotationId, cancellationToken);
            if (quotation is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.QuotationId), "Invalid Id"));
            }

            var item = await _itemService.GetByCode(request.ItemCode, cancellationToken);
            if (item is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.ItemCode), "Invalid ItemCode"));
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            var items = quotation.Items;
            var newLineId = items.Max(i => i.LineId) + 1;

            var newItem = new QuotationItem
            {
                LineId = newLineId,
                LineOrder = request.LineOrder ?? newLineId,
                ItemId = item.Id,
                ItemCode = request.ItemCode,
                ItemName = item.Name,
                ReferenceCode = "ref",
                AnvisaCode = item.AnvisaCode ?? string.Empty,
                AnvisaDueDate = item.AnvisaDueDate?.ToUniversalTime() ?? DateTime.Now.ToUniversalTime(),
                UnitPrice = request.UnitPrice,
                Quantity = request.Quantity,
            };

            quotation.AddItem(newItem);

            await _quotationService.Update(quotation, cancellationToken);

            return quotation;
        }
    }
}