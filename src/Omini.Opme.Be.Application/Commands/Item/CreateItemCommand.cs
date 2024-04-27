using MediatR;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record CreateItemCommand : IRequest<Result<Item>>
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

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Result<Item>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        public CreateItemCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(){
                AnvisaCode = request.AnvisaCode,
                AnvisaDueDate = request.AnvisaDueDate,
                Code = request.Code,
                Cst = request.Cst,
                Description = request.Description,
                Id = Guid.NewGuid(),
                Name = request.Name,
                NcmCode = request.NcmCode,
                SalesName = request.SalesName,
                SupplierCode= request.SupplierCode,
                SusCode = request.SusCode,
                Uom = request.Uom
            };

            await _itemRepository.Create(item);
            await _unitOfWork.Commit();

            return item;
        }
    }
}