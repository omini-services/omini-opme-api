using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record UpdateItemCommand : ICommand<Item>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string SalesName { get; set; }
    public string Description { get; set; }
    public string Uom { get; set; }
    public string AnvisaCode { get; set; }
    public DateTime? AnvisaDueDate { get; set; }
    public string SupplierCode { get; set; }
    public string Cst { get; set; }
    public string SusCode { get; set; }
    public string NcmCode { get; set; }


    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, Item>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        public UpdateItemCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item, ValidationResult>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByCode(request.Code, cancellationToken);
            if (item is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            item.SetData(
                name: request.Name,
                salesName: request.SalesName,
                description: request.Description,
                uom: request.Uom,
                anvisaCode: request.AnvisaCode,
                anvisaDueDate: request.AnvisaDueDate,
                supplierCode: request.SupplierCode,
                cst: request.Cst,
                susCode: request.SusCode,
                ncmCode: request.NcmCode
            );

            _itemRepository.Update(item, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return item;
        }
    }
}