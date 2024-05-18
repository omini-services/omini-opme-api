using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdateQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; set; }
    public int LineId { get; set; }
    public int? LineOrder { get; set; }
    public Guid ItemId { get; set; }
    public string ItemCode { get; set; }
    public string AnvisaCode { get; set; }
    public DateTime AnvisaDueDate { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }

    public class UpdateQuotationCommandHandler : ICommandHandler<UpdateQuotationItemCommand, Quotation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IQuotationRepository _quotationRepository;

        public UpdateQuotationCommandHandler(IUnitOfWork unitOfWork,
                                             IItemRepository itemRepository,
                                             IQuotationRepository quotationRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _quotationRepository = quotationRepository;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(UpdateQuotationItemCommand request, CancellationToken cancellationToken)
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

            var item = await _itemRepository.GetById(request.ItemId);
            if (item is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.ItemId), "Invalid Id"));
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            quotationItem.LineId = request.LineId;
            quotationItem.LineOrder = request.LineOrder ?? quotationItem.LineOrder;
            quotationItem.ItemId = request.ItemId;
            quotationItem.ItemCode = request.ItemCode;
            quotationItem.AnvisaCode = request.AnvisaCode;
            quotationItem.AnvisaDueDate = request.AnvisaDueDate.ToUniversalTime();
            quotationItem.UnitPrice = request.UnitPrice;
            quotationItem.Quantity = request.Quantity;
            quotationItem.ItemTotal = request.Quantity * request.UnitPrice;

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}