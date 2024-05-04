using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Omini.Opme.Be.Shared;

namespace AuthenticationConnector;

internal sealed class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger _logger;
    private readonly ApiConnector _apiConnector;

    public AuthenticationMiddleware(ILoggerFactory loggerFactory, IOptions<ApiConnector> apiConnectorOptions)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationMiddleware>();
        _apiConnector = apiConnectorOptions.Value;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        if (GetRequiredHeaders(requestData.Headers))
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                await CreateInternalErrorResult(context);
            }
        }
        else
        {
            await CreateUnauthorizedResult(context, requestData);
        }
    }

    private async Task CreateInternalErrorResult(FunctionContext context)
    {
        var requestData = await context.GetHttpRequestDataAsync();

        if (requestData != null)
        {
            var newHttpResponse = requestData.CreateResponse(HttpStatusCode.Unauthorized);
            await newHttpResponse.WriteAsJsonAsync(new { message = "Internal error" }, newHttpResponse.StatusCode);

            var invocationResult = context.GetInvocationResult();
            invocationResult.Value = newHttpResponse;

            var httpOutputBindingFromMultipleOutputBindings = context.GetOutputBindings<HttpResponseData>()
                .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");
            if (httpOutputBindingFromMultipleOutputBindings is not null)
            {
                httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
            }
            else
            {
                invocationResult.Value = newHttpResponse;
            }
        }
    }

    private async Task CreateUnauthorizedResult(FunctionContext context, HttpRequestData requestData)
    {
        var newHttpResponse = requestData.CreateResponse(HttpStatusCode.Unauthorized);
        await newHttpResponse.WriteAsJsonAsync(new { message = "Unauthorized" }, newHttpResponse.StatusCode);

        var invocationResult = context.GetInvocationResult();
        invocationResult.Value = newHttpResponse;

        var httpOutputBindingFromMultipleOutputBindings = context.GetOutputBindings<HttpResponseData>()
            .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");
        if (httpOutputBindingFromMultipleOutputBindings is not null)
        {
            httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
        }
        else
        {
            invocationResult.Value = newHttpResponse;
        }
    }

    private bool GetRequiredHeaders(HttpHeadersCollection header)
    {
        string authorization;
        if (header.TryGetValues("Authorization", out var values))
        {
            authorization = values.First();
        }
        else
        {
            _logger.LogWarning("Missing HTTP basic authentication header.");
            return false;
        }

        var auth = header.SingleOrDefault(p => p.Key == "Authorization").Value.FirstOrDefault();

        if (!authorization.StartsWith("Basic "))
        {
            _logger.LogWarning("HTTP basic authentication header must start with 'Basic '.");
            return false;
        }
        _logger.LogInformation(System.Environment.GetEnvironmentVariable("CUSTOMCONNSTR_DefaultConnection"));
        _logger.LogInformation(_apiConnector.SignInSignUpExtension.BasicAuthUsername + _apiConnector.SignInSignUpExtension.BasicAuthPassword);

        // Get the the HTTP basinc authorization credentials
        var cred = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth[6..])).Split(':');

        _logger.LogInformation(cred[0] + cred[1]);

        return
            cred[0] == _apiConnector.SignInSignUpExtension.BasicAuthUsername
            && cred[1] == _apiConnector.SignInSignUpExtension.BasicAuthPassword;
    }
}