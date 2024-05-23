
using FluentValidation;
using Omini.Opme.Api.Extensions;

namespace Omini.Opme.Middlewares;

internal static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}

internal class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var error = ex.ToProblemDetails();
            await context.Response.WriteAsJsonAsync(error);
        }
        catch
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            throw;
        }
    }
}