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
public class InsuranceCompaniesController : MainController
{
    private readonly ILogger<InsuranceCompaniesController> _logger;
    public InsuranceCompaniesController(ILogger<InsuranceCompaniesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<InsuranceCompanyOutputDto>>> Get([FromQuery] QueryFilter queryFilter, [FromQuery] PaginationFilter paginationFilter)
    {
        var insuranceCompanies = await Mediator.Send(new GetAllInsuranceCompaniesQuery(queryFilter, paginationFilter));
        var result = Mapper.Map<PagedResult<InsuranceCompanyOutputDto>>(insuranceCompanies);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<InsuranceCompanyOutputDto>> GetByCode([FromServices] IInsuranceCompanyRepository repository, string code)
    {
        var insuranceCompany = await repository.GetByCode(code);

        if (insuranceCompany is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<InsuranceCompanyOutputDto>(insuranceCompany);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateInsuranceCompanyCommand createInsuranceCompanyCommand)
    {
        var result = await Mediator.Send(createInsuranceCompanyCommand);

        return ToCreatedAtRoute(result, Mapper.Map<InsuranceCompanyOutputDto>, nameof(InsuranceCompaniesController), nameof(this.GetByCode), (mapped) => new { code = mapped.Code });
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, [FromBody] UpdateInsuranceCompanyCommand updateInsuranceCompanyCommand)
    {
        if (updateInsuranceCompanyCommand.Code != code)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Code", "Invalid code")]));
        }

        var result = await Mediator.Send(updateInsuranceCompanyCommand);

        return ToOk(result, Mapper.Map<InsuranceCompanyOutputDto>);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var command = new DeleteInsuranceCompanyCommand()
        {
            Code = code
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<InsuranceCompanyOutputDto>);
    }
}
