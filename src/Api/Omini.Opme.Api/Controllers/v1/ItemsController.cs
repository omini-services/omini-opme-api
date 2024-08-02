using Asp.Versioning;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Queries;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
public class ItemsController : MainController
{
    private readonly ILogger<ItemsController> _logger;
    public ItemsController(ILogger<ItemsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ItemOutputDto>>> Get([FromQuery] QueryFilter queryFilter, [FromQuery] PaginationFilter paginationFilter)
    {
        var items = await Mediator.Send(new GetAllItemsQuery(queryFilter, paginationFilter));
        var result = Mapper.Map<PagedResult<ItemOutputDto>>(items);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<ItemOutputDto>> GetByCode([FromServices] IItemRepository repository, string code)
    {
        var item = await repository.GetByCode(code);

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

        return ToCreatedAtRoute(result, Mapper.Map<ItemOutputDto>, nameof(ItemsController), nameof(this.GetByCode), (mapped) => new { code = mapped.Code });
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, [FromBody] UpdateItemCommand updateItemCommand)
    {
        if (updateItemCommand.Code != code)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Code", "Invalid code")]));
        }

        var result = await Mediator.Send(updateItemCommand);

        return ToOk(result, Mapper.Map<ItemOutputDto>);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var command = new DeleteItemCommand()
        {
            Code = code
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<ItemOutputDto>);
    }
}
