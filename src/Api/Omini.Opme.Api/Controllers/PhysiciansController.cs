using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Queries;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

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
    public async Task<ActionResult<PagedResult<PhysicianOutputDto>>> Get([FromQuery] QueryFilter queryFilter, [FromQuery] PaginationFilter paginationFilter)
    {
        var physicians = await Mediator.Send(new GetAllPhysiciansQuery(queryFilter, paginationFilter));
        var result = Mapper.Map<PagedResult<PhysicianOutputDto>>(physicians);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<PhysicianOutputDto>> GetByCode([FromServices] IPhysicianRepository repository, string code)
    {
        var physician = await repository.GetByCode(code);

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

        return ToCreatedAtRoute(result, Mapper.Map<PhysicianOutputDto>, nameof(PhysiciansController), nameof(this.GetByCode), (mapped) => new { code = mapped.Code });
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, [FromBody] UpdatePhysicianCommand updatePhysicianCommand)
    {
        if (updatePhysicianCommand.Code != code)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Code", "Invalid code")]));
        }

        var result = await Mediator.Send(updatePhysicianCommand);

        return ToOk(result, Mapper.Map<PhysicianOutputDto>);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var command = new DeletePhysicianCommand()
        {
            Code = code
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<PhysicianOutputDto>);
    }
}
