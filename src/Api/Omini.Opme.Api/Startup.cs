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

        services.AddAuthenticationConfiguration(Configuration);

        services.AddInfrastructure(Configuration);

        services.AddApplication();

        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddVersionConfiguration();

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
        app.UseSerilogRequestLogging();
        app.UseLoggingMiddleware();
        app.UseExceptionMiddleware();

        app.UseQuestPdf((options) =>
        {
            options.FontsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "fonts");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}