
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Application.Commands;
using Omini.Opme.Be.Domain.Repositories;

namespace Omini.Opme.Be.Api.Controllers;

[ApiController]
[Route($"{Constants.ApiPath}/[controller]")]
public class ItemsController : MainController
{
    private readonly ILogger<ItemsController> _logger;
    public ItemsController(ILogger<ItemsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<ItemOutputDto>>> Get([FromServices] IItemRepository repository)
    {
        var items = await repository.GetAll();
        var result = Mapper.Map<IList<ItemOutputDto>>(items);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpGet("{id:guid}", Name = "GetById")]
    public async Task<ActionResult<ItemOutputDto>> GetById([FromServices] IItemRepository repository, Guid id)
    {
        var items = await repository.GetById(id);
        var result = Mapper.Map<ItemOutputDto>(items);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] ItemCreateDto itemCreateDto)
    {
        var command = new CreateItemCommand()
        {
            Code = itemCreateDto.Code,
            Name = itemCreateDto.Name,
            SalesName = itemCreateDto.SalesName,
            Description = itemCreateDto.Description,
            Uom = itemCreateDto.Uom,
            AnvisaCode = itemCreateDto.AnvisaCode,
            AnvisaDueDate = itemCreateDto.AnvisaDueDate,
            SupplierCode = itemCreateDto.SupplierCode,
            Cst = itemCreateDto.Cst,
            SusCode = itemCreateDto.SusCode,
            NcmCode = itemCreateDto.NcmCode
        };

        var result = await Mediator.Send(command);

        var newItem = Mapper.Map<ItemOutputDto>(result.Response);

        return CreatedAtRoute("GetById", new { id = newItem.Id }, ResponseDto.ApiSuccess(newItem));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ItemUpdateDto itemUpdateDto)
    {
        if (itemUpdateDto.Id != id)
        {
            throw new ValidationException("invalid id", new List<ValidationFailure>() { new ValidationFailure("id", "invalid id") });
        }

        var command = new UpdateItemCommand()
        {
            Code = itemUpdateDto.Code,
            Name = itemUpdateDto.Name,
            SalesName = itemUpdateDto.SalesName,
            Description = itemUpdateDto.Description,
            Id = itemUpdateDto.Id,
            Uom = itemUpdateDto.Uom,
            AnvisaCode = itemUpdateDto.AnvisaCode,
            AnvisaDueDate = itemUpdateDto.AnvisaDueDate,
            SupplierCode = itemUpdateDto.SupplierCode,
            Cst = itemUpdateDto.Cst,
            SusCode = itemUpdateDto.SusCode,
            NcmCode = itemUpdateDto.NcmCode
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {       
        var command = new DeleteItemCommand(){
            Id = id
        };
        await Mediator.Send(command);

        return NoContent();
    }
}
