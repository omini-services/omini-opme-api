using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Extensions;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Api.Controllers;

public class MainController : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    private IMapper _mapper;

    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>()!;

    public IActionResult ToOk<TResult, TContract>(Result<TResult, ValidationException> result, Func<TResult, TContract> mapper){
        return result.Match(obj => {
            var response = mapper(obj);
            return Ok(ResponseDto.ApiSuccess(response));
        }, ToBadRequest);
    }

    public IActionResult ToCreatedAtRoute<TResult, TContract>(Result<TResult, ValidationException> result, Func<TResult, TContract> mapper, string? controllerName, string? actionName, Func<TContract, object>? routeValues){
        return result.Match(obj => {
            var response = mapper(obj);
            return CreatedAtAction(actionName, controllerName[..^10], routeValues is not null ? routeValues(response) : null, ResponseDto.ApiSuccess(response));            
        }, ToBadRequest);
    }

    public IActionResult ToNoContent<TResult>(Result<TResult, ValidationException> result){
        return result.Match(obj => {
            return NoContent();
        }, ToBadRequest);
    }

    public IActionResult ToBadRequest(ValidationException validationException){
        return new BadRequestObjectResult(validationException.ToProblemDetails());
    }
}