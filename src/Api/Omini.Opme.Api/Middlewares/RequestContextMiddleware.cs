

using Omini.Opme.Shared.Services.Security;
using Serilog;

namespace Omini.Opme.Api.Middlewares;

internal static class RequestContextMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestContextMiddleware>();
    }
}

internal class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IClaimsService _claimsService;
    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger, IDiagnosticContext diagnosticContext, IClaimsService claimsService)
    {
        _next = next;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        _claimsService = claimsService;
    }

    public Task Invoke(HttpContext context)
    {
        _diagnosticContext.Set("UserId", _claimsService.OpmeUserId);
        _diagnosticContext.Set("UserEmail", _claimsService.Email);

        return _next(context);
    }
}