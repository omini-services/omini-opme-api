using BeforeSignUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Omini.Opme.Be.Infrastructure;
using Omini.Opme.Be.Shared.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((worker) =>
    {
        worker.UseMiddleware<AuthenticationMiddleware>();
    })
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        builder.AddJsonFile("local.settings.json");
        builder.AddConfiguration(hostContext.Configuration);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure(hostContext.Configuration);
        services.AddScoped<IClaimsService, ClaimsService>();
    })
    .Build();

host.Run();
