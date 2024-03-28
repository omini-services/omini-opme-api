using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

[ApiController]
[Route("[controller]")]
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

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemOutputDto>> GetById([FromServices] IItemRepository repository, Guid id)
    {
        var items = await repository.GetById(id);
        var result = Mapper.Map<ItemOutputDto>(items);

        return Ok(result);
    }

        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] ItemCreateDto itemCreateDto)
        {
            var command = new ItemCreateCommand(createExpenseGroupDto.Name);

            var result = await Mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var newExpenseGroup = Mapper.Map<ExpenseGroupDto>((ExpenseGroup)result.Data);

            return CreatedAtRoute("GetExpenseGroup", new { id = newExpenseGroup.Id }, newExpenseGroup);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateExpenseGroupDto updateExpenseGroupDto)
        {
            if (updateExpenseGroupDto.Id != id)
            {
                return BadRequest();
            }

            var command = new UpdateExpenseGroupCommand(updateExpenseGroupDto.Id, updateExpenseGroupDto.Name);
            var result = await Mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }
}
