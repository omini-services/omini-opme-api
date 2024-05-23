using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Transactions;
using Omini.Opme.Infrastructure.Contexts;
using Omini.Opme.Infrastructure.PdfGenerator.QuestPdf;
using Omini.Opme.Infrastructure.Repositories;
using Omini.Opme.Infrastructure.Services;
using Omini.Opme.Infrastructure.Transaction;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OpmeContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IDateTimeService, DateTimeService>();

        services.AddTransient<IOpmeUserRepository, OpmeUserRepository>();

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