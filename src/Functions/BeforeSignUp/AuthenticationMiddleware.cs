using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Omini.Opme.Be.Shared;

namespace BeforeSignUp;

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
            await next(context);
        }
        else
        {
            var newHttpResponse = requestData.CreateResponse(HttpStatusCode.Unauthorized);
            await newHttpResponse.WriteAsJsonAsync(newHttpResponse.StatusCode);

            var invocationResult = context.GetInvocationResult();
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

        if (authorization.StartsWith("Basic "))
        {
            _logger.LogWarning("HTTP basic authentication header must start with 'Basic '.");
            return false;
        }

        // Get the the HTTP basinc authorization credentials
        var cred = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth[6..])).Split(':');

        return
            cred[0] == _apiConnector.SignInSignUpExtension.BasicAuthUsername
            && cred[1] == _apiConnector.SignInSignUpExtension.BasicAuthPassword;
    }
}