using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Extensions;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers.V1;

[ApiVersion(1)]
[ApiController]
public class MainController : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    private IMapper _mapper;

    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>()!;

    protected IActionResult ToOk<TResult, TContract>(Result<TResult, ValidationResult> result, Func<TResult, TContract> mapper){
        return result.Match(obj => {
            var response = mapper(obj);
            return Ok(ResponseDto.ApiSuccess(response));
        }, ToBadRequest);
    }

    protected IActionResult ToCreatedAtRoute<TResult, TContract>(Result<TResult, ValidationResult> result, Func<TResult, TContract> mapper, string? controllerName, string? actionName, Func<TContract, object>? routeValues){
        return result.Match(obj => {
            var response = mapper(obj);
            return CreatedAtAction(actionName, controllerName[..^10], routeValues is not null ? routeValues(response) : null, ResponseDto.ApiSuccess(response));            
        }, ToBadRequest);
    }

    protected IActionResult ToNoContent<TResult>(Result<TResult, ValidationResult> result){
        return result.Match(obj => {
            return NoContent();
        }, ToBadRequest);
    }

    protected IActionResult ToBadRequest(ValidationResult validationResult){
        return new BadRequestObjectResult(new ValidationException(validationResult.Errors).ToProblemDetails());
    }
}