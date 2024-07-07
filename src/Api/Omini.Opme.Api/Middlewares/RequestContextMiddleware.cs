

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
    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        return _next(context);
        // using (LogContext.PushProperty("correlationId", context.TraceIdentifier))
        // {
        //     return _next(context);
        // }
    }
}