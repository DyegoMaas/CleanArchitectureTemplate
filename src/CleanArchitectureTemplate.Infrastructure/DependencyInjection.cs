using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Domain.Services;
using CleanArchitectureTemplate.Infrastructure.FileSystemStorage;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Repositories;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Infrastructure
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
                .AddClasses(classes => classes.AssignableTo<IRepository>())
                .AsMatchingInterface()
                .WithTransientLifetime()
            );

            return services;
        }
    }
}