using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        public UpdateItemCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item, ValidationResult>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetById(request.Id, cancellationToken);
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

            await _unitOfWork.Commit(cancellationToken);

            return item;
        }
    }
}