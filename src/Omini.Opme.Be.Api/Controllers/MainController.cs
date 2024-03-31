using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Omini.Opme.Be.Api.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

    private IMapper _mapper;

    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
}