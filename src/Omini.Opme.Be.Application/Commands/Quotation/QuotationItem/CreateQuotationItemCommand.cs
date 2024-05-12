using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreateQuotationItemCommand : ICommand<QuotationItem>
{
    public Guid QuotationId { get; set; }
    public int? LineOrder { get; set; }
    public Guid ItemId { get; set; }
    public string ItemCode { get; set; }
    public string AnvisaCode { get; set; }
    public DateTime AnvisaDueDate { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }

    public class CreateQuotationItemCommandHandler : ICommandHandler<CreateQuotationItemCommand, QuotationItem>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IQuotationRepository _quotationRepository;

        public CreateQuotationItemCommandHandler(IUnitOfWork unitOfWork,
                                                 IItemRepository itemRepository,
                                                 IQuotationRepository quotationRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _quotationRepository = quotationRepository;
        }

        public async Task<Result<QuotationItem, ValidationResult>> Handle(CreateQuotationItemCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationRepository.GetById(request.QuotationId, cancellationToken);
            if (quotation is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.QuotationId), "Invalid Id"));
            }

            var item = await _itemRepository.GetById(request.ItemId);
            if (item is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.ItemId), "Invalid Id"));
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
                ItemId = request.ItemId,
                ItemCode = request.ItemCode,
                AnvisaCode = request.AnvisaCode,
                AnvisaDueDate = request.AnvisaDueDate,
                UnitPrice = request.UnitPrice,
                ItemTotal = request.ItemTotal,
                Quantity = request.Quantity,
            };

            items.Add(newItem);

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return newItem;
        }
    }
}