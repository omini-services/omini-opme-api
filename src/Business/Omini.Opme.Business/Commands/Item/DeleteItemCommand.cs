using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;
namespace Omini.Opme.Business.Commands;

public record DeleteItemCommand : ICommand<Item>
{
    public string Code { get; init; }

    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, Item>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Item, ValidationResult>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByCode(request.Code, cancellationToken);
            if (item is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Code), "Invalid code")]);
            }

            _itemRepository.Delete(item, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return item;
        }
    }
}