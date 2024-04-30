
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemOutputDto>> GetById([FromServices] IItemRepository repository, Guid id)
    {
        var items = await repository.GetById(id);
        var result = Mapper.Map<ItemOutputDto>(items);

        return Ok(ResponseDto.ApiSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
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

        return ToCreatedAtRoute(result, Mapper.Map<ItemOutputDto>, nameof(ItemsController), nameof(this.GetById), (mapped) => new { id = mapped.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ItemUpdateDto itemUpdateDto)
    {
        if (itemUpdateDto.Id != id)
        {
            return ToBadRequest(new ValidationException("Invalid id", new List<ValidationFailure>() { new ValidationFailure("Id", "Invalid id") }));
        }

        var command = new UpdateItemCommand()
        {
            Id = itemUpdateDto.Id,
            Code = itemUpdateDto.Code,
            Name = itemUpdateDto.Name,
            SalesName = itemUpdateDto.SalesName,
            Description = itemUpdateDto.Description,
            Uom = itemUpdateDto.Uom,
            AnvisaCode = itemUpdateDto.AnvisaCode,
            AnvisaDueDate = itemUpdateDto.AnvisaDueDate,
            SupplierCode = itemUpdateDto.SupplierCode,
            Cst = itemUpdateDto.Cst,
            SusCode = itemUpdateDto.SusCode,
            NcmCode = itemUpdateDto.NcmCode
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteItemCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return ToNoContent(result);
    }
}
