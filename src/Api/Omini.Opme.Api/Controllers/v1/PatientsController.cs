using Asp.Versioning;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Queries;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers.V1;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class PatientsController : MainController
{
    private readonly ILogger<PatientsController> _logger;
    public PatientsController(ILogger<PatientsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PatientOutputDto>>> Get([FromQuery] QueryFilter queryFilter, [FromQuery] PaginationFilter paginationFilter)
    {
        var patients = await Mediator.Send(new GetAllPatientsQuery(queryFilter, paginationFilter));
        var result = Mapper.Map<PagedResult<PatientOutputDto>>(patients);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<PatientOutputDto>> GetByCode([FromServices] IPatientRepository repository, string code)
    {
        var patient = await repository.GetByCode(code);

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

        return ToCreatedAtRoute(result, Mapper.Map<PatientOutputDto>, nameof(PatientsController), nameof(this.GetByCode), (mapped) => new { code = mapped.Code });
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, [FromBody] UpdatePatientCommand updatePatientCommand)
    {
        if (updatePatientCommand.Code != code)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var result = await Mediator.Send(updatePatientCommand);

        return ToOk(result, Mapper.Map<PatientOutputDto>);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var command = new DeletePatientCommand()
        {
            Code = code
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<PatientOutputDto>);
    }
}
