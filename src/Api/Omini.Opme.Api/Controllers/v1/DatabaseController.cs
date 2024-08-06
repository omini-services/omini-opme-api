using Microsoft.AspNetCore.Mvc;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
public class DatabaseController : MainController
{
    private readonly ILogger<DatabaseController> _logger;
    public DatabaseController(ILogger<DatabaseController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Prepare")]
    public async Task<ActionResult> Prepare()
    {
        await Mediator.Send(new PrepareDatabaseCommand());

        return NoContent();
    }
}
