using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; set; }
    public int LineId { get; set; }
    public int? LineOrder { get; set; }
    public string ItemCode { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }

    public class UpdateQuotationCommandHandler : ICommandHandler<UpdateQuotationItemCommand, Quotation>
    {
        private readonly IItemService _itemService;
        private readonly IQuotationService _quotationService;

        public UpdateQuotationCommandHandler(IItemService itemService,
                                             IQuotationService quotationService)
        {
            _itemService = itemService;
            _quotationService = quotationService;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(UpdateQuotationItemCommand request, CancellationToken cancellationToken)
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

            var item = await _itemService.GetByCode(request.ItemCode);
            if (item is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.ItemCode), "Invalid ItemCode"));
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            quotationItem.LineId = request.LineId;
            quotationItem.LineOrder = request.LineOrder ?? quotationItem.LineOrder;
            quotationItem.ItemId = item.Id;
            quotationItem.ItemCode = request.ItemCode;
            quotationItem.ItemName = item.Name;
            quotationItem.ReferenceCode = "ref";
            quotationItem.AnvisaCode = item.AnvisaCode ?? string.Empty;
            quotationItem.AnvisaDueDate = item.AnvisaDueDate?.ToUniversalTime() ?? DateTime.Now.ToUniversalTime();
            quotationItem.UnitPrice = request.UnitPrice;
            quotationItem.Quantity = request.Quantity;
            quotationItem.ItemTotal = request.Quantity * request.UnitPrice;

            quotation.Total = quotation.Items.Sum(p => p.ItemTotal);

            await _quotationService.Update(quotation, cancellationToken);

            return quotation;
        }
    }
}