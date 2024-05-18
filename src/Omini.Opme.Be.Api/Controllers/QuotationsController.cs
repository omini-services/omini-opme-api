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
                Quantity = item.Quantity,
            })
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<QuotationOutputDto>, nameof(QuotationsController), nameof(this.GetById), (mapped) => new
        {
            id = mapped.Id
        });
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> CreateItem(Guid id,
     [FromBody] QuotationCreateLineItemDto quotationCreateItemDto)
    {
        if (quotationCreateItemDto.QuotationId != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var command = new CreateQuotationItemCommand()
        {
            QuotationId = quotationCreateItemDto.QuotationId,
            LineOrder = quotationCreateItemDto.LineOrder,
            ItemId = quotationCreateItemDto.ItemId,
            ItemCode = quotationCreateItemDto.ItemCode,
            AnvisaCode = quotationCreateItemDto.AnvisaCode,
            AnvisaDueDate = quotationCreateItemDto.AnvisaDueDate,
            UnitPrice = quotationCreateItemDto.UnitPrice,
            Quantity = quotationCreateItemDto.Quantity,
        };

        var result = await Mediator.Send(command);

        return ToCreatedAtRoute(result, Mapper.Map<QuotationOutputDto>, nameof(QuotationsController), nameof(this.GetById), (mapped) => new
        {
            id = mapped.Id
        });
    }

    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> Update(Guid id, [FromBody] QuotationUpdateDto quotationUpdateDto)
    // {
    //     if (quotationUpdateDto.Id != id)
    //     {
    //         return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
    //     }

    //     var command = new UpdateQuotationCommand()
    //     {
    //         Id = quotationUpdateDto.Id,
    //         Number = quotationUpdateDto.Number,
    //         PatientId = quotationUpdateDto.PatientId,
    //         PhysicianId = quotationUpdateDto.PhysicianId,
    //         PayingSourceType = quotationUpdateDto.PayingSourceType,
    //         PayingSourceId = quotationUpdateDto.PayingSourceId,
    //         HospitalId = quotationUpdateDto.HospitalId,
    //         InsuranceCompanyId = quotationUpdateDto.InsuranceCompanyId,
    //         InternalSpecialistId = quotationUpdateDto.InternalSpecialistId,
    //         DueDate = quotationUpdateDto.DueDate,
    //         Items = quotationUpdateDto.Items.Select((item, index) => new UpdateQuotationCommand.UpdateQuotationItemCommand()
    //         {
    //             LineId = item.LineId,
    //             LineOrder = item.LineOrder,
    //             ItemId = item.ItemId,
    //             ItemCode = item.ItemCode,
    //             AnvisaCode = item.AnvisaCode,
    //             AnvisaDueDate = item.AnvisaDueDate,
    //             UnitPrice = item.UnitPrice,
    //             Quantity = item.Quantity,
    //         })
    //     };

    //     var result = await Mediator.Send(command);

    //     return ToNoContent(result);
    // }

    [HttpPut("{id:guid}/items/{lineId:int}")]
    public async Task<IActionResult> UpdateItem(Guid id, int lineId, [FromBody] QuotationUpdateLineItemDto quotationUpdateItemDto)
    {
        if (quotationUpdateItemDto.QuotationId != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        if (quotationUpdateItemDto.LineId != lineId)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("LineId", "Invalid id")]));
        }

        var command = new UpdateQuotationItemCommand()
        {
            QuotationId = id,
            LineId = quotationUpdateItemDto.LineId,
            LineOrder = quotationUpdateItemDto.LineOrder,
            ItemId = quotationUpdateItemDto.ItemId,
            ItemCode = quotationUpdateItemDto.ItemCode,
            AnvisaCode = quotationUpdateItemDto.AnvisaCode,
            AnvisaDueDate = quotationUpdateItemDto.AnvisaDueDate,
            UnitPrice = quotationUpdateItemDto.UnitPrice,
            Quantity = quotationUpdateItemDto.Quantity,
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

    [HttpDelete("{id:guid}/items/{lineId:int}")]
    public async Task<IActionResult> DeleteItem(Guid id, int lineId)
    {
        var command = new DeleteQuotationItemCommand()
        {
            QuotationId = id,
            LineId = lineId
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
