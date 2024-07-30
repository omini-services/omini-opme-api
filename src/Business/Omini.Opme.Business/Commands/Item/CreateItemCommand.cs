using FluentValidation.Results;
using Omini.Opme.Business.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;
public record CreateItemCommand : ICommand<Item>
{
    public string Name { get; init; }
    public string SalesName { get; init; }
    public string Description { get; init; }
    public string Uom { get; init; }
    public string AnvisaCode { get; init; }
    public DateTime? AnvisaDueDate { get; init; }
    public string SupplierCode { get; init; }
    public string Cst { get; init; }
    public string SusCode { get; init; }
    public string NcmCode { get; init; }

    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Item>
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IValidator<CreateItemCommand> _validator;
        private readonly IItemRepository _itemRepository;
        public CreateItemCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            //_validator = validator;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item, ValidationResult>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request);
            //if (validationResult.IsValid)
            //{
            //    return new ValidationException(validationResult.Errors);
            //}

            var item = new Item(
                anvisaCode: request.AnvisaCode,
                anvisaDueDate: request.AnvisaDueDate,
                cst: request.Cst,
                description: request.Description,
                name: request.Name,
                ncmCode: request.NcmCode,
                salesName: request.SalesName,
                supplierCode: request.SupplierCode,
                susCode: request.SusCode,
                uom: request.Uom
            );

            await _itemRepository.Add(item, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return item;
        }
    }
}