using Autoplace.Autoparts.Consumer;
using Autoplace.Autoparts.Data;
using Autoplace.Autoparts.Services;
using Autoplace.Common.Extensions;
using Autoplace.Common.Services.Data;
using Autoplace.Common.Services.Files;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddWebServices<AutopartsDbContext>(builder.Configuration)
    .AddMessaging(builder.Configuration, typeof(ChangeAutopartStatusConsumer))
    .AddScoped<IAutopartsService, AutopartsService>()
    .AddLogging(config => config.AddConsole())
    .AddSingleton(typeof(ILogger), typeof(Logger<Program>))
    .AddScoped<IDataSeeder, DataSeeder>()
    .AddScoped<IImageService, FileSystemImageSaverService>();
    
var app = builder.Build();

app.UseWebService()
   .Initialize()
   .Run();
