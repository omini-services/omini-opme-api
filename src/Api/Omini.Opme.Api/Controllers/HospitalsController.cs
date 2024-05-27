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
public class HospitalsController : MainController
{
    private readonly ILogger<HospitalsController> _logger;
    public HospitalsController(ILogger<HospitalsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<HospitalOutputDto>>> Get([FromQuery]PaginationFilter paginationFilter, [FromServices] IHospitalRepository repository)
    {
        var hospitals = await Mediator.Send(new GetAllHospitalsQuery(paginationFilter));
        var result = Mapper.Map<PagedResult<HospitalOutputDto>>(hospitals);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HospitalOutputDto>> GetById([FromServices] IHospitalRepository repository, Guid id)
    {
        var hospital = await repository.GetById(id);

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

        return ToCreatedAtRoute(result, Mapper.Map<HospitalOutputDto>, nameof(HospitalsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHospitalCommand updateHospitalCommand)
    {
        if (updateHospitalCommand.Id != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var result = await Mediator.Send(updateHospitalCommand);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteHospitalCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
