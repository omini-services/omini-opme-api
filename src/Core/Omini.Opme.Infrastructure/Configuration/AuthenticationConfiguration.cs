using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Omini.Opme.Infrastructure.Configuration;

internal static class AuthenticationConfiguration
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth0:Authority"];
            options.Audience = configuration["Auth0:Audience"];
        });

        return services;
    }
}
