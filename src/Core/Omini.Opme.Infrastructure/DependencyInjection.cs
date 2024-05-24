using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Services.Pdf;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Infrastructure.Contexts;
using Omini.Opme.Infrastructure.Interceptors;
using Omini.Opme.Infrastructure.Pdf.QuestPdf;
using Omini.Opme.Infrastructure.Repositories;
using Omini.Opme.Infrastructure.Services;
using Omini.Opme.Infrastructure.Transaction;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<EntityInterceptor>();
        services.AddSingleton<AuditableInterceptor>();
        services.AddSingleton<SoftDeletableInterceptor>();

        services.AddDbContext<OpmeContext>((sp, opt) => 
        {
            opt.AddInterceptors(
                sp.GetRequiredService<EntityInterceptor>(),
                sp.GetRequiredService<AuditableInterceptor>(),
                sp.GetRequiredService<SoftDeletableInterceptor>()
            );
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IDateTimeService, DateTimeService>();

        services.AddScoped<IOpmeUserRepository, OpmeUserRepository>();

        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IInsuranceCompanyRepository, InsuranceCompanyRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPhysicianRepository, PhysicianRepository>();
        services.AddScoped<IQuotationRepository, QuotationRepository>();

        services.AddScoped<IQuotationPdfGenerator, QuotationPdfGenerator>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IApplicationBuilder UseQuestPdf(this IApplicationBuilder app, Action<QuestPdfBuilder> options)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<IApplicationBuilder>>();

        QuestPDF.Settings.License = LicenseType.Community;
        var questPdfBuilder = new QuestPdfBuilder();

        options(questPdfBuilder);
        
        if (questPdfBuilder.FontsPath is not null)
        {
            QuestPdfConfiguration.RegisterFontsFromPath(questPdfBuilder.FontsPath, logger);
        }

        return app;
    }
}