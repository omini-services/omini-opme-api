using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;

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
        private readonly IIdentityOpmeUserRepository _identityOpmeUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BeforeSignUp(ILoggerFactory loggerFactory, IIdentityOpmeUserRepository identityOpmeUserRepository, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<BeforeSignUp>();
            _identityOpmeUserRepository = identityOpmeUserRepository;
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

            await _identityOpmeUserRepository.Create(new Omini.Opme.Be.Domain.Entities.IdentityOpmeUser()
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
