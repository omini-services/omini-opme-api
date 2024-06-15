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
    public class User
    {
        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public Dictionary<string, object> UserMetadata { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateOpmeUser
    {
        private readonly ILogger _logger;
        private readonly IOpmeUserRepository _opmeUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOpmeUser(ILoggerFactory loggerFactory, IOpmeUserRepository opmeUserRepository, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<CreateOpmeUser>();
            _opmeUserRepository = opmeUserRepository;
            _unitOfWork = unitOfWork;
        }

        [Function("CreateOpmeUser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function create opme user");

            var data = await JsonSerializer.DeserializeAsync<User>(req.Body,
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
