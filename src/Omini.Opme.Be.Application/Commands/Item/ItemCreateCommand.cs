using MediatR;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record ItemCreateCommand : IRequest<Result<Item, ValidationFailed>>
{
    public ItemCreateCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public class ItemCreateCommandHandler : IRequestHandler<ItemCreateCommand, Result<Item, ValidationFailed>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        public ItemCreateCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public Task<Result<Item, ValidationFailed>> Handle(ItemCreateCommand request, CancellationToken cancellationToken)
        {
             var expenseGroup = new ExpenseGroup(request.Name);
                //foreach (var expenseCommand in command.Expenses)
                //{
                //    expenseGroup.AddExpense(new ExpenseGroupChildren(expenseCommand.Name, expenseCommand.ExtItemCode, expenseCommand.ExtUsage));
                //}

                await _itemRepository.Create(expenseGroup);
                await _unitOfWork.Commit();

                return Result.Success(expenseGroup);
        }
    }
}