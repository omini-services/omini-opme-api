
using FluentValidation;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Middlewares;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}

public class ExceptionMiddleware
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
        catch (Exception ex)
        {
            ResponseDto response;

            if (ex is ValidationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationException = ex as ValidationException;
                response = ResponseDto.ApiError(validationException.Errors.Select(p => p.ToString()).ToArray());
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = ResponseDto.ApiError("An error occurred whilst processing your request");
            }

            await context.Response.WriteAsJsonAsync(response);

            throw;
        }
    }
}