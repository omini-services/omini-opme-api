using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateItemCommand : ICommand<Item>
{
    public Guid Id { get; init; }
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


    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, Item>
    {
        private readonly IItemService _itemService;
        public UpdateItemCommandHandler(IItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task<Result<Item, ValidationResult>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemService.GetById(request.Id, cancellationToken);
            if (item is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            item.AnvisaCode = request.AnvisaCode;
            item.AnvisaDueDate = request.AnvisaDueDate;
            item.Code = request.Code;
            item.Cst = request.Cst;
            item.Description = request.Description;
            item.Name = request.Name;
            item.NcmCode = request.NcmCode;
            item.SalesName = request.SalesName;
            item.SupplierCode = request.SupplierCode;
            item.SusCode = request.SusCode;
            item.Uom = request.Uom;

            await _itemService.Update(item, cancellationToken);

            return item;
        }
    }
}