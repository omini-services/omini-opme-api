
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class QuotationsController : MainController
{
    private readonly ILogger<QuotationsController> _logger;
    public QuotationsController(ILogger<QuotationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<QuotationOutputDto>>> Get([FromServices] IQuotationRepository repository)
    {
        var quotations = await repository.GetAll();
        var result = Mapper.Map<IList<QuotationOutputDto>>(quotations);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<QuotationOutputDto>> GetById([FromServices] IQuotationRepository repository, Guid id)
    {
        var quotation = await repository.GetById(id);

        if (quotation is null)
        {
            return BadRequest();
        }

        var result = Mapper.Map<QuotationOutputDto>(quotation);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] QuotationCreateDto quotationCreateDto)
    {
        var command = new CreateQuotationCommand()
        {
            Number = quotationCreateDto.Number,
            PatientId = quotationCreateDto.PatientId,
            PhysicianId = quotationCreateDto.PhysicianId,
            PayingSourceType = quotationCreateDto.PayingSourceType,
            PayingSourceId = quotationCreateDto.PayingSourceId,
            HospitalId = quotationCreateDto.HospitalId,
            InsuranceCompanyId = quotationCreateDto.InsuranceCompanyId,
            InternalSpecialistId = quotationCreateDto.InternalSpecialistId,
            DueDate = quotationCreateDto.DueDate,
            Items = quotationCreateDto.Items.Select((item, index) => new CreateQuotationCommand.CreateQuotationItemCommand()
            {
                LineOrder = item.LineOrder,
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                AnvisaCode = item.AnvisaCode,
                AnvisaDueDate = item.AnvisaDueDate,
                UnitPrice = item.UnitPrice,
                ItemTotal = item.ItemTotal,
                Quantity = item.Quantity,
            })
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<QuotationOutputDto>, nameof(QuotationsController), nameof(this.GetById), (mapped) => new
        {
            id = mapped.Id
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] QuotationUpdateDto quotationUpdateDto)
    {
        if (quotationUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdateQuotationCommand()
        {
            Id = quotationUpdateDto.Id,
            Number = quotationUpdateDto.Number,
            PatientId = quotationUpdateDto.PatientId,
            PhysicianId = quotationUpdateDto.PhysicianId,
            PayingSourceType = quotationUpdateDto.PayingSourceType,
            PayingSourceId = quotationUpdateDto.PayingSourceId,
            HospitalId = quotationUpdateDto.HospitalId,
            InsuranceCompanyId = quotationUpdateDto.InsuranceCompanyId,
            InternalSpecialistId = quotationUpdateDto.InternalSpecialistId,
            DueDate = quotationUpdateDto.DueDate,
            Items = quotationUpdateDto.Items.Select((item, index) => new UpdateQuotationCommand.UpdateQuotationItemCommand()
            {
                LineId = item.LineId,
                LineOrder = item.LineOrder,
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                AnvisaCode = item.AnvisaCode,
                AnvisaDueDate = item.AnvisaDueDate,
                UnitPrice = item.UnitPrice,
                ItemTotal = item.ItemTotal,
                Quantity = item.Quantity,
            })
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteQuotationCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
