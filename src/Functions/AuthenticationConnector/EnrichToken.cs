using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Omini.Opme.Domain.Repositories;

namespace AuthenticationConnector
{
    public class EnrichRequest
    {
        public string Email { get; set; }
    }


    public class EnrichToken
    {
        private readonly ILogger _logger;
        private readonly IOpmeUserRepository _opmeUserRepository;

        public EnrichToken(ILoggerFactory loggerFactory, IOpmeUserRepository opmeUserRepository)
        {
            _logger = loggerFactory.CreateLogger<CreateOpmeUser>();
            _opmeUserRepository = opmeUserRepository;
        }

        [Function("EnrichToken")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function enrichToken");

            _logger.LogInformation("Searching for user mapped");

            var data = await JsonSerializer.DeserializeAsync<EnrichRequest>(req.Body,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            var opmeUser = await _opmeUserRepository.FindByEmail(data.Email);

            HttpResponseData response;

            if (opmeUser is not null)
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                var claims = new Dictionary<string, string>()
                {
                    {"opmeUserId", opmeUser.Id.ToString()}
                };

                var transformedClaims = claims.Select(entry => new
                {
                    key = entry.Key,
                    value = entry.Value
                }).ToArray();

                await response.WriteAsJsonAsync(
                    transformedClaims.ToArray(),
                    response.StatusCode
                );
            }
            else
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new
                {
                    userMessage = "Could not find user in Opme"
                }, response.StatusCode);
            }

            return response;
        }
    }
}
