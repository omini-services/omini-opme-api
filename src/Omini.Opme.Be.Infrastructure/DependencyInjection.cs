using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Omini.Opme.Be.Application.Services;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Infrastructure.Contexts;
using Omini.Opme.Be.Infrastructure.Repositories;
using Omini.Opme.Be.Infrastructure.Services;
using Omini.Opme.Be.Infrastructure.Transaction;

namespace Omini.Opme.Be.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OpmeContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IAuditableService, AuditableService>();

        services.AddTransient<IIdentityOpmeUserRepository, IdentityOpmeUserRepository>();

        services.AddTransient<IHospitalRepository, HospitalRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<IInsuranceCompanyRepository, InsuranceCompanyRepository>();
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IPhysicianRepository, PhysicianRepository>();
        services.AddTransient<IQuotationRepository, QuotationRepository>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddQuestPdfGenerator(this IServiceCollection services, string fontsPath)
    {    
        if (fontsPath is not null)
        {
            QuestPdfGenerator.RegisterFontsFromPath(fontsPath);
        }

        services.AddTransient<IPdfGenerator, QuotationPdfGenerator>();
        
        return services;
    }
}