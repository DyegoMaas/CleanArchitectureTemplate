using System.Linq;
using System.Reflection;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Infrastructure
{
    public static class DependencyInjection
    {
        private static readonly ConfiguredOnce _configuredOnce = new();

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMongoDb();
            services.AddRepositories();
            
            return services;
        }

        private static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            ConfigureOnce();
            
            services.AddSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();
            services.AddSingleton(serviceProvider =>
            {
                var mongoDatabaseFactory = serviceProvider.GetRequiredService<IMongoDatabaseFactory>();
                var mongoDatabase = mongoDatabaseFactory.GetDatabase();
                MappingLoader.LoadClassMappings();
                MappingLoader.EnsureIndices(mongoDatabase);
                SerializationConventions.SetConventions();
                return mongoDatabase;
            });
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var repositories = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IMongoDbRepository).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(concreteType =>
                {
                    var interfaces = concreteType.GetInterfaces()
                        .Where(i => i != typeof(IMongoDbRepository))
                        .Select(i => (abstractType: i, concreteType: concreteType))
                        .Union(new [] {(abstractType: concreteType, concreteType: concreteType)});
                    return interfaces;
                })
                .ToArray();

            foreach (var (abstractType, concreteType) in repositories)
            {
                services.AddTransient(abstractType, concreteType);
            }

            return services;
        }
        
        private class ConfiguredOnce
        {
            public bool HasRun { get; set; }
        }
        
        private static void ConfigureOnce()
        {
            lock (_configuredOnce)
            {
                if (_configuredOnce.HasRun)
                    return;

                SerializationConventions.SetConventions();
                _configuredOnce.HasRun = true;
            }
        }
    }
}