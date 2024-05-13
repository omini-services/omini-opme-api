using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Omini.Opme.Be.Api.Tests.Authentication;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Api.Tests;

public abstract class IntegrationTest
{
    protected readonly HttpClient TestClient;

    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(OpmeContext));
                    services.AddDbContext<OpmeContext>(options =>
                    {
                        options.UseInMemoryDatabase("testDb");
                    });

                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    }).AddJwtBearer("TestScheme", options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = JwtTokenProvider.Issuer,
                            ValidAudience = JwtTokenProvider.Issuer
                        };
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    });
                });
            });

        TestClient = appFactory.CreateClient();
    }

    protected void Authenticate(Func<string>? GetToken = null)
    {
        string bearer;
        if (GetToken is null)
        {
            bearer = new TestJwtToken().WithOpme(Guid.NewGuid()).Build();
        }
        else
        {
            bearer = GetToken();
        }

        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, bearer);
    }
}