namespace AutoPlace.Web.Infrastructure
{
    using AutoPlace.Data;
    using AutoPlace.Data.Common;
    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Repositories;
    using AutoPlace.Services;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Administration;
    using AutoPlace.Services.Messaging;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(
            this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Register services
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IEmailSender, NullMessageSender>();
            services.AddScoped<ITextService, TextService>();

            services.AddTransient<IAutopartsService, AutopartsService>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IContactFormsService, ContactFormsService>();
            services.AddTransient(typeof(IItemsService<>), typeof(ItemsService<>));
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IVotesService, VotesService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IAutopartsCharacteristicsService, AutopartsCharacteristicsService>();

            return services;
        }
    }
}
