using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Api.Tests;

internal class IntegrationTest
{
    protected readonly HttpClient TestClient;

    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(OpmeContext));
                    services.AddDbContext<OpmeContext>(options =>
                    {
                        options.UseInMemoryDatabase("testDb");
                    });
                    services.Configure<JwtBearerOptions>(
                        JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                        options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    }
                    );
                });
            });

        TestClient = appFactory.CreateClient();
    }

    protected async Task Authenticate()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, await GetJwt());
    }

    private async Task GetJwt()
    {
        var response = TestClient.PostAsJsonAsync()
    }
}