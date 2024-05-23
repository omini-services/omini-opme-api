using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Domain.Repositories;

namespace Omini.Opme.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class PhysiciansController : MainController
{
    private readonly ILogger<PhysiciansController> _logger;
    public PhysiciansController(ILogger<PhysiciansController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<PhysicianOutputDto>>> Get([FromServices] IPhysicianRepository repository)
    {
        var physicians = await repository.GetAll();
        var result = Mapper.Map<IList<PhysicianOutputDto>>(physicians);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PhysicianOutputDto>> GetById([FromServices] IPhysicianRepository repository, Guid id)
    {
        var physician = await repository.GetById(id);

        if (physician is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<PhysicianOutputDto>(physician);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePhysicianCommand createPhysicianCommand)
    {
        var result = await Mediator.Send(createPhysicianCommand);

        return ToCreatedAtRoute(result, Mapper.Map<PhysicianOutputDto>, nameof(PhysiciansController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePhysicianCommand updatePhysicianCommand)
    {
        if (updatePhysicianCommand.Id != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var result = await Mediator.Send(updatePhysicianCommand);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeletePhysicianCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
