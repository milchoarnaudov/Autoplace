using Autoplace.Administration.Consumers;
using Autoplace.Administration.Data;
using Autoplace.Administration.Services;
using Autoplace.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWebServices<AdministrationDbContext>(builder.Configuration)
    .AddMessaging(builder.Configuration, typeof(ApprovalRequestConsumer))
    .AddLogging(config => config.AddConsole())
    .AddSingleton(typeof(ILogger), typeof(Logger<Program>))
    .AddScoped<IApprovalRequestsService, ApprovalRequestsService>();

var app = builder.Build();

app.UseWebService()
   .Initialize()
   .Run();
