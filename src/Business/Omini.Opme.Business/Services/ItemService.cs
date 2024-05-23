using System.Linq.Expressions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Business;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ItemService(IItemRepository itemRepository, IUnitOfWork unitOfWork)
    {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Item> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _itemRepository.GetById(id, cancellationToken);
    }

    public async Task<Item> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await _itemRepository.GetByCode(code, cancellationToken);
    }

    public async Task<IEnumerable<Item>> Get(Expression<Func<Item, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _itemRepository.Get(predicate, cancellationToken);
    }

    public async Task<Item> Add(Item item, CancellationToken cancellationToken = default)
    {
        await _itemRepository.Add(item, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return item;
    }

    public async Task<Item> Update(Item item, CancellationToken cancellationToken = default)
    {
        var itemExists = await _itemRepository.GetById(item.Id, cancellationToken);
        if (itemExists is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(item.Id), "Invalid id")]);
        }

        _itemRepository.Update(item, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return item;
    }

    public async Task<Item> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var Item = await _itemRepository.GetById(id, cancellationToken);
        if (Item is null)
        {
            //return new ValidationResult([new ValidationFailure(nameof(id), "Invalid id")]);
        }

        //_auditableService.SoftDelete(Item);

        _itemRepository.Delete(id, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Item;
    }
}