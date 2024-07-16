using Omini.Opme.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.With<HighlightLogEnricher>()
    .Enrich.WithEnvironmentName()
    .Enrich.FromLogContext()
      .WriteTo.Async(async =>
        async.OpenTelemetry(options =>
        {
            options.Endpoint = HighlightConfig.LogsEndpoint;
            options.Protocol = HighlightConfig.Protocol;
            options.ResourceAttributes = HighlightConfig.ResourceAttributes;
        })
    )
    .ReadFrom.Configuration(ctx.Configuration));
HighlightConfig.Configure(builder);

builder.Configuration.AddAzureAppConfiguration(options =>
  options.Connect(builder.Configuration.GetConnectionString("AppConfig")).UseFeatureFlags(featureFlagOptions =>
  {
      //featureFlagOptions.Select("TestApp:*", "dev");
      featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(60);
  })
);

startup.ConfigureServices(builder, builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Error(
        "The following {Exception} was thrown during application startup",
        ex);
}
finally
{
    Log.CloseAndFlush();
}
