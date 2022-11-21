using Autoplace.Common.Extensions;
using Autoplace.Common.Services.Messaging;
using Autoplace.Members.Consumers;
using Autoplace.Members.Data;
using Autoplace.Members.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWebServices<MembersDbContext>(builder.Configuration)
    .AddMessaging(builder.Configuration, typeof(UserRegisteredConsumer))
    .AddLogging(config => config.AddConsole())
    .AddSingleton(typeof(ILogger), typeof(Logger<Program>))
    .AddScoped<IMembersService, MembersService>()
    .AddScoped<IChatService, ChatService>();

var app = builder.Build();

app.UseWebService()
   .Initialize()
   .Run();
