using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Omini.Opme.Be.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}