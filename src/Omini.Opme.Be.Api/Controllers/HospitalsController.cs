
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

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
    public async Task<ActionResult<IList<HospitalOutputDto>>> Get([FromServices] IHospitalRepository repository)
    {
        var hospitals = await repository.GetAll();
        var result = Mapper.Map<IList<HospitalOutputDto>>(hospitals);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HospitalOutputDto>> GetById([FromServices] IHospitalRepository repository, Guid id)
    {
        var hospital = await repository.GetById(id);

        if (hospital is null){
            return BadRequest();
        }
        
        var result = Mapper.Map<HospitalOutputDto>(hospital);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] HospitalCreateDto hospitalCreateDto)
    {
        var command = new CreateHospitalCommand()
        {
            Cnpj = hospitalCreateDto.Cnpj,
            LegalName = hospitalCreateDto.LegalName,
            TradeName = hospitalCreateDto.TradeName,
            Comments = hospitalCreateDto.Comments,
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<HospitalOutputDto>, nameof(HospitalsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HospitalUpdateDto hospitalUpdateDto)
    {
        if (hospitalUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdateHospitalCommand()
        {
            Id = hospitalUpdateDto.Id,
            Cnpj = hospitalUpdateDto.Cnpj,
            LegalName = hospitalUpdateDto.LegalName,
            TradeName = hospitalUpdateDto.TradeName,
            Comments = hospitalUpdateDto.Comments,
        };

        var result = await Mediator.Send(command);

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
