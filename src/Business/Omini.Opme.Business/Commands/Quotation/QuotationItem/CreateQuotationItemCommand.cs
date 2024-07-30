using FluentValidation.Results;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Business.Abstractions.Messaging;

namespace Omini.Opme.Business.Commands;

public record CreateQuotationItemCommand : ICommand<Quotation>
{
    public Guid QuotationId { get; set; }
    public int? LineOrder { get; set; }
    public string ItemCode { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }

    public class CreateQuotationItemCommandHandler : ICommandHandler<CreateQuotationItemCommand, Quotation>
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

        public async Task<Result<Quotation, ValidationResult>> Handle(CreateQuotationItemCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationRepository.GetById(request.QuotationId, cancellationToken);
            if (quotation is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.QuotationId), "Invalid quotation id"));
            }

            var item = await _itemRepository.GetByCode(request.ItemCode, cancellationToken);
            if (item is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.ItemCode), "Invalid item code"));
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            quotation.AddItem(
                itemCode: item.Code,
                itemName: item.Name,
                referenceCode: "ref",
                anvisaCode: item.AnvisaCode ?? string.Empty,
                anvisaDueDate: item.AnvisaDueDate ?? DateTime.Now,
                unitPrice: request.UnitPrice,
                quantity: request.Quantity
            );

            _quotationRepository.Update(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}