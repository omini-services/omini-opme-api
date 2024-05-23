using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Shared.Entities;
namespace Omini.Opme.Business.Commands;

public record DeleteItemCommand : ICommand<Item>
{
    public Guid Id { get; init; }

    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, Item>
    {
        private readonly IItemService _itemService;

        public DeleteItemCommandHandler(IItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task<Result<Item, ValidationResult>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemService.GetById(request.Id, cancellationToken);
            if (item is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _itemService.Delete(item.Id, cancellationToken);

            return item;
        }
    }
}