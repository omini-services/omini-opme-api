
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class InsuranceCompaniesController : MainController
{
    private readonly ILogger<InsuranceCompaniesController> _logger;
    public InsuranceCompaniesController(ILogger<InsuranceCompaniesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<InsuranceCompanyOutputDto>>> Get([FromServices] IInsuranceCompanyRepository repository)
    {
        var insuranceCompanies = await repository.GetAll();
        var result = Mapper.Map<IList<InsuranceCompanyOutputDto>>(insuranceCompanies);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InsuranceCompanyOutputDto>> GetById([FromServices] IInsuranceCompanyRepository repository, Guid id)
    {
        var insuranceCompany = await repository.GetById(id);

        if (insuranceCompany is null){
            return BadRequest();
        }        

        var result = Mapper.Map<InsuranceCompanyOutputDto>(insuranceCompany);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] InsuranceCompanyCreateDto insuranceCompanyCreateDto)
    {
        var command = new CreateInsuranceCompanyCommand()
        {
            Cnpj = insuranceCompanyCreateDto.Cnpj,
            LegalName = insuranceCompanyCreateDto.LegalName,
            TradeName = insuranceCompanyCreateDto.TradeName,
            Comments = insuranceCompanyCreateDto.Comments,
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<InsuranceCompanyOutputDto>, nameof(InsuranceCompaniesController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] InsuranceCompanyUpdateDto insuranceCompanyUpdateDto)
    {
        if (insuranceCompanyUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdateInsuranceCompanyCommand()
        {
            Id = insuranceCompanyUpdateDto.Id,
            Cnpj = insuranceCompanyUpdateDto.Cnpj,
            LegalName = insuranceCompanyUpdateDto.LegalName,
            TradeName = insuranceCompanyUpdateDto.TradeName,
            Comments = insuranceCompanyUpdateDto.Comments,
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteInsuranceCompanyCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
