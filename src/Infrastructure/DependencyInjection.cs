using Domain.Repositories;
using Domain.Services;
using Infrastructure.Database.Common;
using Infrastructure.Database.Common.Serialization;
using Infrastructure.Database.Repositories;
using Infrastructure.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMongoDb();
            services.AddRepositories();
            services.AddSingleton<IBookStorageConfiguration, BookStorageConfigurationEnv>();
            services.AddSingleton<IBookContentRepository, BookContentRepository>();
            services.AddSingleton<IUpdateBookReferenceService, BookMetadataRepository>();
            
            return services;
        }

        private static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();
            services.AddSingleton(serviceProvider =>
            {
                SerializationConventions.SetConventions();

                var mongoDatabaseFactory = serviceProvider.GetRequiredService<IMongoDatabaseFactory>();
                var mongoDatabase = mongoDatabaseFactory.GetDatabase();
                MappingLoader.LoadClassMappings();
                MappingLoader.EnsureIndices(mongoDatabase);
                return mongoDatabase;
            });
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IMongoDbRepository>()
                .AddClasses(classes => classes.AssignableTo<IRepository>())
                .AsMatchingInterface()
                .WithTransientLifetime()
            );

            return services;
        }
    }
}