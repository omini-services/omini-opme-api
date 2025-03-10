using Asp.Versioning;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Application.Commands;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Queries;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
public class QuotationsController : MainController
{
    private readonly ILogger<QuotationsController> _logger;
    public QuotationsController(ILogger<QuotationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<QuotationOutputDto>>> Get([FromQuery] string queryValue, [FromQuery] PaginationFilter paginationFilter)
    {
        var quotations = await Mediator.Send(new GetAllQuotationsQuery(queryValue, paginationFilter));
        var result = Mapper.Map<PagedResult<QuotationOutputDto>>(quotations);

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


    [HttpGet("{id:guid}/preview")]
    public async Task<ActionResult<QuotationOutputDto>> PreviewById([FromServices] IQuotationRepository repository, Guid id)
    {
        var quotation = await repository.GetById(id);

        if (quotation is null)
        {
            return BadRequest();
        }

        var previewQuotationByIdCommand = new PreviewQuotationCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(previewQuotationByIdCommand);

        return File(result.Response!, "application/pdf");
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateQuotationCommand createQuotationCommand)
    {
        var result = await Mediator.Send(createQuotationCommand);

        return ToCreatedAtRoute(result, Mapper.Map<QuotationOutputDto>, nameof(QuotationsController), nameof(this.GetById), (mapped) => new
        {
            id = mapped.Id
        });
    }

    [HttpPost("{quotationId:guid}/items")]
    public async Task<IActionResult> CreateItem(Guid quotationId,
     [FromBody] CreateQuotationItemCommand createQuotationItemCommand)
    {
        if (createQuotationItemCommand.QuotationId != quotationId)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("QuotationId", "Invalid quotation id")]));
        }

        var result = await Mediator.Send(createQuotationItemCommand);

        return ToCreatedAtRoute(result, Mapper.Map<QuotationOutputDto>, nameof(QuotationsController), nameof(this.GetById), (mapped) => new
        {
            id = mapped.Id
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuotationCommand updateQuotationCommand)
    {
        if (updateQuotationCommand.Id != id)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("Id", "Invalid id")]));
        }

        var result = await Mediator.Send(updateQuotationCommand);

        return ToOk(result, Mapper.Map<QuotationOutputDto>);
    }

    [HttpPut("{quotationId:guid}/items/{lineId:int}")]
    public async Task<IActionResult> UpdateItem(Guid quotationId, int lineId, [FromBody] UpdateQuotationItemCommand updateQuotationItemCommand)
    {
        if (updateQuotationItemCommand.QuotationId != quotationId)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("QuotationId", "Invalid quotation id")]));
        }

        if (updateQuotationItemCommand.LineId != lineId)
        {
            return ToBadRequest(new ValidationResult([new ValidationFailure("LineId", "Invalid id")]));
        }

        var result = await Mediator.Send(updateQuotationItemCommand);

        return ToOk(result, Mapper.Map<QuotationOutputDto>);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteQuotationCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<QuotationOutputDto>);
    }

    [HttpDelete("{quotationId:guid}/items/{lineId:int}")]
    public async Task<IActionResult> DeleteItem(Guid quotationId, int lineId)
    {
        var command = new DeleteQuotationItemCommand()
        {
            QuotationId = quotationId,
            LineId = lineId
        };

        var result = await Mediator.Send(command);

        return ToOk(result, Mapper.Map<QuotationOutputDto>);
    }
}
