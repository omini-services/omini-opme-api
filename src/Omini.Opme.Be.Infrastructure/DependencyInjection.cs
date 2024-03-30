using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Infrastructure.Contexts;
using Omini.Opme.Be.Infrastructure.Repositories;
using Omini.Opme.Be.Infrastructure.Transaction;

namespace Omini.Opme.Be.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OpmeContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IItemRepository, ItemRepository>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}