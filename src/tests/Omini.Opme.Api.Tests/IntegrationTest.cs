using System.Text.Json;
using System.Text.Json.Serialization;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Omini.Opme.Be.Api.Tests.Authentication;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Api.Tests;

public abstract class IntegrationTest
{
    protected readonly FlurlClient TestClient;

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

        TestClient = new FlurlClient(appFactory.CreateClient()).WithSettings(settings =>
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            settings.JsonSerializer = new DefaultJsonSerializer(jsonOptions);
        });
    }
}