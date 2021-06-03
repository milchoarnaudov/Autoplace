namespace AutoPlace.Web.Infrastructure
{
    using System.Linq;
    using System.Reflection;

    using AutoPlace.Services.Common;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConventionalServices(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            var singletonServiceInterfaceType = typeof(ISingletonService);
            var scopedServiceInterfaceType = typeof(IScopedService);
            var transientServiceInterfaceType = typeof(ITransientService);

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes()
               .Where(t => t.IsClass && !t.IsAbstract)
               .Select(t => new
               {
                   Service = t.GetInterface($"I{t.Name}"),
                   Implementation = t,
               })
               .Where(t => t.Service != null);

                foreach (var type in types)
                {
                    if (transientServiceInterfaceType.IsAssignableFrom(type.Service))
                    {
                        services.AddTransient(type.Service, type.Implementation);
                    }
                    else if (scopedServiceInterfaceType.IsAssignableFrom(type.Service))
                    {
                        services.AddScoped(type.Service, type.Implementation);
                    }
                    else if (singletonServiceInterfaceType.IsAssignableFrom(type.Service))
                    {
                        services.AddSingleton(type.Service, type.Implementation);
                    }
                }
            }

            return services;
        }
    }
}
