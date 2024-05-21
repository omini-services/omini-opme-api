using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Domain.Transactions;
using Omini.Opme.Be.Infrastructure.Contexts;
using Omini.Opme.Be.Infrastructure.PdfGenerator.QuestPdf;
using Omini.Opme.Be.Infrastructure.Repositories;
using Omini.Opme.Be.Infrastructure.Services;
using Omini.Opme.Be.Infrastructure.Transaction;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Be.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OpmeContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IAuditableService, AuditableService>();
        services.AddTransient<IDateTimeService, DateTimeService>();

        services.AddTransient<IIdentityOpmeUserRepository, IdentityOpmeUserRepository>();

        services.AddTransient<IHospitalRepository, HospitalRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<IInsuranceCompanyRepository, InsuranceCompanyRepository>();
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IPhysicianRepository, PhysicianRepository>();
        services.AddTransient<IQuotationRepository, QuotationRepository>();

        services.AddTransient<IQuotationPdfGenerator, QuotationPdfGenerator>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

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