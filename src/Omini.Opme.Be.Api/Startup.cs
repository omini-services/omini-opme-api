using System.Reflection;
using FluentValidation;
using MediatR;
using Omini.Opme.Be.Application;
using Omini.Opme.Be.Application.PipelineBehaviors;
using Omini.Opme.Be.Infrastructure;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();

        services.AddInfrastructure(Configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

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

        app.UseHttpsRedirection();

        // app.UseRequestLogging();
        // app.UseGlobalException();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}