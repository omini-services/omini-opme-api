using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Omini.Opme.Be.Api.Configuration;
using Omini.Opme.Be.Application;
using Omini.Opme.Be.Infrastructure;
using Omini.Opme.Be.Middlewares;

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
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });
        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();

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
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseLoggingMiddleware();
        app.UseExceptionMiddleware();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}