
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

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
    public async Task<ActionResult<IList<PatientOutputDto>>> Get([FromServices] IPatientRepository repository)
    {
        var patients = await repository.GetAll();
        var result = Mapper.Map<IList<PatientOutputDto>>(patients);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientOutputDto>> GetById([FromServices] IPatientRepository repository, Guid id)
    {
        var patient = await repository.GetById(id);
    
        if (patient is null){
            return BadRequest();
        }        

        var result = Mapper.Map<PatientOutputDto>(patient);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] PatientCreateDto patientCreateDto)
    {
        var command = new CreatePatientCommand()
        {
            Cpf = patientCreateDto.Cpf,
            FirstName = patientCreateDto.FirstName,
            MiddleName = patientCreateDto.MiddleName,
            LastName = patientCreateDto.LastName,
            Comments = patientCreateDto.Comments,
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<PatientOutputDto>, nameof(PatientsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PatientUpdateDto patientUpdateDto)
    {
        if (patientUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdatePatientCommand()
        {
            Id = patientUpdateDto.Id,
            Cpf = patientUpdateDto.Cpf,
            FirstName = patientUpdateDto.FirstName,
            MiddleName = patientUpdateDto.MiddleName,
            LastName = patientUpdateDto.LastName,
            Comments = patientUpdateDto.Comments,
        };

        var result = await Mediator.Send(command);

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
