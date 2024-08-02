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
public class HospitalsController : MainController
{
    private readonly ILogger<HospitalsController> _logger;
    public HospitalsController(ILogger<HospitalsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ResponsePagedDto<HospitalOutputDto>>> Get([FromQuery] QueryFilter queryFilter, [FromQuery] PaginationFilter paginationFilter)
    {
        var hospitals = await Mediator.Send(new GetAllHospitalsQuery(queryFilter, paginationFilter));
        var result = Mapper.Map<PagedResult<HospitalOutputDto>>(hospitals);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<HospitalOutputDto>> GetByCode([FromServices] IHospitalRepository repository, string code)
    {
        var hospital = await repository.GetByCode(code);

        if (hospital is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<HospitalOutputDto>(hospital);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateHospitalCommand createHospitalCommand)
    {
        var result = await Mediator.Send(createHospitalCommand);

        return ToCreatedAtRoute(result, Mapper.Map<HospitalOutputDto>, nameof(HospitalsController), nameof(this.GetByCode), (mapped) => new { code = mapped.Code });
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, [FromBody] UpdateHospitalCommand updateHospitalCommand)
    {
        if (updateHospitalCommand.Code != code)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Code", "Invalid code")]));
        }

        var result = await Mediator.Send(updateHospitalCommand);

        return ToOk(result, Mapper.Map<HospitalOutputDto>);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var command = new DeleteHospitalCommand()
        {
            Code = code
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<HospitalOutputDto>);
    }
}
