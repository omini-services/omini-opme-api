
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

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
        var physicians = await repository.GetById(id);
        var result = Mapper.Map<PhysicianOutputDto>(physicians);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] PhysicianCreateDto physicianCreateDto)
    {
        var command = new CreatePhysicianCommand()
        {
            Cro = physicianCreateDto.Cro,
            Crm = physicianCreateDto.Crm,
            FirstName = physicianCreateDto.FirstName,
            MiddleName = physicianCreateDto.MiddleName,
            LastName = physicianCreateDto.LastName,
            Comments = physicianCreateDto.Comments,
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<PhysicianOutputDto>, nameof(PhysiciansController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PhysicianUpdateDto physicianUpdateDto)
    {
        if (physicianUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdatePhysicianCommand()
        {
            Id = physicianUpdateDto.Id,
            Cro = physicianUpdateDto.Cro,
            Crm = physicianUpdateDto.Crm,
            FirstName = physicianUpdateDto.FirstName,
            MiddleName = physicianUpdateDto.MiddleName,
            LastName = physicianUpdateDto.LastName,
            Comments = physicianUpdateDto.Comments,
        };

        var result = await Mediator.Send(command);

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
