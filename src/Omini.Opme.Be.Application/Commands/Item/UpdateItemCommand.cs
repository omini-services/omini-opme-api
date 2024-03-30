using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record UpdateItemCommand : IRequest<Result<Item, ValidationFailed>>
{
    public Guid Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string SalesName { get; init; }
    public string Description { get; init; }
    public string Uom { get; init; }
    public string AnvisaCode { get; init; }
    public DateTime AnvisaDueDate { get; init; }
    public string SupplierCode { get; init; }
    public string Cst { get; init; }
    public string SusCode { get; init; }
    public string NcmCode { get; init; }


    public class ItemCreateCommandHandler : IRequestHandler<UpdateItemCommand, Result<Item, ValidationFailed>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        public ItemCreateCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item, ValidationFailed>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetById(request.Id);
            if (item is null)
            {
                return new ValidationFailed(new ValidationFailure("id", "invalid id"));
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

            await _unitOfWork.Commit();

            return item;
        }
    }
}