﻿using AutoMapper;
using Autoplace.Common.Mappings;
using Autoplace.Common.Services.Identity;
using Autoplace.Common.Services.Messaging;
using GreenPipes;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultConnectionString = "DefaultConnection";
        public static IServiceCollection AddWebServices<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TDbContext : DbContext
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddDatabase<TDbContext>(configuration)
                .AddTokenAuthentication(configuration)
                .AddHealth(configuration)
                .AddAutoMapperProfile(Assembly.GetCallingAssembly())
                .Configure<RouteOptions>(options => options.LowercaseUrls = true)
                .AddControllers();

            return services;
        }

        public static IServiceCollection AddDatabase<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TDbContext : DbContext
            => services
                .AddScoped<DbContext, TDbContext>()
                .AddDbContext<TDbContext>(options => options
                    .UseSqlServer(
                        configuration.GetConnectionString(DefaultConnectionString),
                        sqlOptions => sqlOptions
                            .EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null)));

        public static IServiceCollection AddTokenAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            JwtBearerEvents events = null)
        {
            var secret = configuration
                .GetSection("ApplicationSettings")
                .GetValue<string>("Secret");

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    if (events != null)
                    {
                        bearer.Events = events;
                    }
                });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddAutoMapperProfile(
            this IServiceCollection services,
            Assembly assembly)
            => services
                .AddAutoMapper(
                    (_, config) => config
                        .AddProfile(new MappingProfile(assembly)),
                    Array.Empty<Assembly>());

        public static IServiceCollection AddHealth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var healthChecks = services.AddHealthChecks();

            healthChecks
                .AddSqlServer(configuration.GetConnectionString(DefaultConnectionString));

            healthChecks
                .AddRabbitMQ(rabbitConnectionString: "amqp://rabbitmq:rabbitmq@rabbitmq/");

            return services;
        }

        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration configuration,
            params Type[] consumers)
        {
            services
                .AddTransient<IPublisher, Publisher>()
                .AddTransient<IMessageService, MessageService>()
                .AddMassTransit(mt =>
                {
                    foreach (var consumer in consumers)
                    {
                        mt.AddConsumer(consumer);
                    }

                    mt.AddBus(context => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        var messageQueueSettings = configuration.GetSection("MessageQueueSettings");

                        var host = messageQueueSettings.GetValue<string>("Host");
                        var username = messageQueueSettings.GetValue<string>("Username"); ;
                        var password = messageQueueSettings.GetValue<string>("Password"); ;

                        rmq.Host(host, host =>
                        {
                            host.Username(username);
                            host.Password(password);
                        });

                        foreach (var consumer in consumers)
                        {
                            rmq.ReceiveEndpoint(consumer.FullName, endpoint =>
                            {
                                endpoint.PrefetchCount = 6;
                                endpoint.UseMessageRetry(retry => retry.Interval(5, 10000));

                                endpoint.ConfigureConsumer(context, consumer);
                            });
                        };
                    }));
                })
                .AddMassTransitHostedService();
                

            return services;
        }
    }
}
