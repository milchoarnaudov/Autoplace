using Autoplace.Autoparts.Data;
using Autoplace.Autoparts.Services;
using Autoplace.Common.Data.Services;
using Autoplace.Common.Extensions;
using AutoPlace.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddWebServices<AutopartsDbContext>(builder.Configuration)
    .AddMessaging(builder.Configuration)
    .AddScoped<IAutopartsService, AutopartsService>()
    .AddLogging(config => config.AddConsole())
    .AddSingleton(typeof(ILogger), typeof(Logger<Program>))
    .AddScoped<IDataSeeder, DataSeeder>()
    .AddScoped<IImageService, FileSystemImageSaverService>();

var app = builder.Build();

app.UseWebService()
   .Initialize()
   .Run();
