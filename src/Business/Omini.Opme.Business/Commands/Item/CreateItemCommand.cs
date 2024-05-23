using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;
public record CreateItemCommand : ICommand<Item>
{
    public string Code { get; init; }
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
        // private readonly IValidator<CreateItemCommand> _validator;
        private readonly IItemService _itemService;
        public CreateItemCommandHandler(IItemService itemService)
        {
            //_validator = validator;
            _itemService = itemService;
        }

        public async Task<Result<Item, ValidationResult>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request);
            //if (validationResult.IsValid)
            //{
            //    return new ValidationException(validationResult.Errors);
            //}

            var item = new Item()
            {
                AnvisaCode = request.AnvisaCode,
                AnvisaDueDate = request.AnvisaDueDate,
                Code = request.Code,
                Cst = request.Cst,
                Description = request.Description,
                Id = Guid.NewGuid(),
                Name = request.Name,
                NcmCode = request.NcmCode,
                SalesName = request.SalesName,
                SupplierCode = request.SupplierCode,
                SusCode = request.SusCode,
                Uom = request.Uom
            };

            await _itemService.Add(item, cancellationToken);

            return item;
        }
    }
}