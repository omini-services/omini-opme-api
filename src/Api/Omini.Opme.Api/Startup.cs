using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Omini.Opme.Api.Configuration;
using Omini.Opme.Api.Middlewares;
using Omini.Opme.Middlewares;
using Omini.Opme.Business;
using Omini.Opme.Infrastructure;
using Omini.Opme.Api.Services.Security;
using Omini.Opme.Shared.Services.Security;
using FluentValidation.AspNetCore;
using Serilog;
using Asp.Versioning.ApiExplorer;
using Microsoft.FeatureManagement;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(WebApplicationBuilder app, IServiceCollection services)
    {
        services.AddFeatureManagement();
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

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IClaimsService, ClaimsService>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfiguration(Configuration);

        services.AddInfrastructure(Configuration);

        services.AddBusiness();

        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddVersionConfiguration();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthUsePkce();
                c.OAuthClientId(Configuration["Auth0:ClientId"]);
                c.OAuthAppName("Swagger Api Calls");
                c.OAuthScopes(Configuration.GetSection("Auth0:Scopes").Get<string[]>());

                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.DocumentTitle = "opme-api";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                }
            });
        }

        //app.UseHttpsRedirection();

        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseExceptionMiddleware();
        app.UseLoggingMiddleware();

        app.UseQuestPdf((options) =>
        {
            options.FontsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "fonts");
        });

        app.UseAuthentication();
        app.UseCors();
        app.UseAuthorization();

        app.UseSerilogRequestLogging();
        app.UseRequestContextMiddleware();


        app.MapControllers();
    }
}