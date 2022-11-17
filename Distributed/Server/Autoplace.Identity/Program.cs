using Autoplace.Common.Data.Services;
using Autoplace.Common.Extensions;
using Autoplace.Identity.Data;
using Autoplace.Identity.Data.Models;
using Autoplace.Identity.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services
    .AddWebServices<IdentityDbContext>(builder.Configuration)
    .AddMessaging(builder.Configuration)
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<ITokenProviderService, TokenProviderService>()
    .AddScoped<IDataSeeder, DataSeeder>();

var app = builder.Build();

app.UseWebService()
    .Initialize()
    .Run();
