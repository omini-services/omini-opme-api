using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class PatientsController : MainController
{
    private readonly ILogger<PatientsController> _logger;
    public PatientsController(ILogger<PatientsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PatientOutputDto>>> Get([FromServices] IPatientRepository repository)
    {
        var patients = await repository.GetAllPaginated();
        var result = Mapper.Map<PagedResult<PatientOutputDto>>(patients);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientOutputDto>> GetById([FromServices] IPatientRepository repository, Guid id)
    {
        var patient = await repository.GetById(id);

        if (patient is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<PatientOutputDto>(patient);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientCommand createPatientCommand)
    {
        var result = await Mediator.Send(createPatientCommand);

        return ToCreatedAtRoute(result, Mapper.Map<PatientOutputDto>, nameof(PatientsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientCommand updatePatientCommand)
    {
        if (updatePatientCommand.Id != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var result = await Mediator.Send(updatePatientCommand);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeletePatientCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
