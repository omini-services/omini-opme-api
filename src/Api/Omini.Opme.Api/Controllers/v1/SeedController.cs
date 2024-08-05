using Asp.Versioning;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Queries;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
public class SeedController : MainController
{
    private readonly ILogger<SeedController> _logger;
    public SeedController(ILogger<SeedController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ResponsePagedDto<HospitalOutputDto>>> Seed()
    {
        return Ok("Not workin yet");
    }
}
