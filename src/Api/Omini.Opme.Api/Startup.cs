using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Omini.Opme.Api.Configuration;
using Omini.Opme.Api.Middlewares;
using Omini.Opme.Api.Security;
using Omini.Opme.Middlewares;
using Omini.Opme.Shared.Interfaces;
using Omini.Opme.Business;
using Omini.Opme.Infrastructure;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(WebApplicationBuilder app, IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddScoped<IClaimsService, ClaimsService>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthenticationConfiguration(Configuration);
        services.AddInfrastructure(Configuration);

        services.AddApplication();

        services.AddAutoMapper(typeof(Startup));
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseLoggingMiddleware();
        app.UseExceptionMiddleware();

        app.UseQuestPdf((options) =>
        {
            options.FontsPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "fonts");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}