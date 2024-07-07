using Serilog;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc
      .WriteTo.Console()
      .ReadFrom.Configuration(ctx.Configuration));

startup.ConfigureServices(builder, builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
