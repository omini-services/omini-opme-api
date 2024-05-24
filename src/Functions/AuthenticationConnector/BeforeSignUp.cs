using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Omini.Opme.Domain.Admin;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;

namespace AuthenticationConnector
{
    public class UserSignUpRequest
    {
        public string Email { get; set; }
        public List<Identity> Identities { get; set; }
    }

    public class Identity
    {
        public string SignInType { get; set; }
        public string Issuer { get; set; }
        public string IssuerAssignedId { get; set; }
    }

    public class BeforeSignUp
    {
        private readonly ILogger _logger;
        private readonly IOpmeUserRepository _opmeUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BeforeSignUp(ILoggerFactory loggerFactory, IOpmeUserRepository opmeUserRepository, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<BeforeSignUp>();
            _opmeUserRepository = opmeUserRepository;
            _unitOfWork = unitOfWork;
        }

        [Function("BeforeSignUp")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function beforeSignUp");

            var data = await JsonSerializer.DeserializeAsync<UserSignUpRequest>(req.Body,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            _logger.LogInformation("Creating user mapping");

            await _opmeUserRepository.Create(new OpmeUser()
            {
                Email = data.Email,
                Id = new Guid()
            });

            await _unitOfWork.Commit();

            _logger.LogInformation($"User created {data.Email}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { version = "1.0.0", action = "Continue" });

            return response;
        }
    }
}
