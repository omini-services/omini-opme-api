using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Domain.Repositories;

namespace Omini.Opme.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class ItemsController : MainController
{
    private readonly ILogger<ItemsController> _logger;
    public ItemsController(ILogger<ItemsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<ItemOutputDto>>> Get([FromServices] IItemRepository repository)
    {
        var items = await repository.GetAll();
        var result = Mapper.Map<IList<ItemOutputDto>>(items);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemOutputDto>> GetById([FromServices] IItemRepository repository, Guid id)
    {
        var item = await repository.GetById(id);

        if (item is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<ItemOutputDto>(item);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateItemCommand createItemCommand)
    {
        var result = await Mediator.Send(createItemCommand);

        return ToCreatedAtRoute(result, Mapper.Map<ItemOutputDto>, nameof(ItemsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemCommand updateItemCommand)
    {
        if (updateItemCommand.Id != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }        

        var result = await Mediator.Send(updateItemCommand);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteItemCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
