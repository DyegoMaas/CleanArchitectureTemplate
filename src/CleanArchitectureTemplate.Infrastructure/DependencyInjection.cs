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
            services.Scan(scan => scan
                .FromAssemblyOf<IMongoDbRepository>()
                .AddClasses(classes => classes.AssignableTo<IMongoDbRepository>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );

            return services;
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

        private class ConfiguredOnce
        {
            public bool HasRun { get; set; }
        }
    }
}