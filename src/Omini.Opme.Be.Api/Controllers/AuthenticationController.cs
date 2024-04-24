
// using FluentValidation;
// using FluentValidation.Results;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using Omini.Opme.Be.Api.Configuration.Models;
// using Omini.Opme.Be.Api.Dtos;
// using Omini.Opme.Be.Application.Commands;
// using Omini.Opme.Be.Domain.Entities;
// using Omini.Opme.Be.Domain.Repositories;

// namespace Omini.Opme.Be.Api.Controllers;

// [ApiController]
// [Route($"{Constants.ApiPath}/[controller]")]
// public class AuthenticationController : MainController
// {
//     private readonly ILogger<ItemsController> _logger;
//     private readonly APIConnectors _apiConnectors;
//     private readonly IIdentityOpmeUserRepository _identityOpmeUserRepository;
//     public AuthenticationController(ILogger<ItemsController> logger, IOptions<APIConnectors> options, IIdentityOpmeUserRepository identityOpmeUserRepository)
//     {
//         _logger = logger;
//         _apiConnectors = options.Value;
//         _identityOpmeUserRepository = identityOpmeUserRepository;
//     }

//     [HttpPost]
//     [AllowAnonymous]
//     public async Task<ActionResult<IList<ItemOutputDto>>> Create(AzureUser azureUser)
//     {
//         await _identityOpmeUserRepository.Create(new IdentityOpmeUser(){
//             Id = Guid.NewGuid(),
//             Email = azureUser.Email
//         });
//         return Ok();
//     }

//     public class Identity
//     {
//         public string SignInType { get; set; }
//         public string Issuer { get; set; }
//         public string IssuerAssignedId { get; set; }
//     }

//     public class AzureUser
//     {
//         public string Email { get; set; }
//        // public List<Identity> Identities { get; set; }
//     }

//     private static bool Authorize(HttpRequest req, ILogger log)
//     {
//         // Get the environment's credentials 
//         string username = System.Environment.GetEnvironmentVariable("BASIC_AUTH_USERNAME", EnvironmentVariableTarget.Process);
//         string password = System.Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD", EnvironmentVariableTarget.Process);

//         // Returns authorized if the username is empty or not exists.
//         if (string.IsNullOrEmpty(username))
//         {
//             log.LogInformation("HTTP basic authentication is not set.");
//             return true;
//         }

//         // Check if the HTTP Authorization header exist
//         if (!req.Headers.ContainsKey("Authorization"))
//         {
//             log.LogWarning("Missing HTTP basic authentication header.");
//             return false;
//         }

//         // Read the authorization header
//         var auth = req.Headers["Authorization"].ToString();

//         // Ensure the type of the authorization header id `Basic`
//         if (!auth.StartsWith("Basic "))
//         {
//             log.LogWarning("HTTP basic authentication header must start with 'Basic '.");
//             return false;
//         }

//         // Get the the HTTP basinc authorization credentials
//         var cred = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');

//         // Evaluate the credentials and return the result
//         return (cred[0] == username && cred[1] == password);
//     }
// }
