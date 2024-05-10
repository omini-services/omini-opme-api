// var builder = WebApplication.CreateBuilder(args);
// var startup = new Startup(builder.Configuration);

// startup.ConfigureServices(builder, builder.Services);

// var app = builder.Build();

// startup.Configure(app, builder.Environment);

// app.Run();


using Omini.Opme.Be.Application.Services;

QuotationPdfGenerator quotationPdfGenerator= new QuotationPdfGenerator();
quotationPdfGenerator.GenerateDocument();