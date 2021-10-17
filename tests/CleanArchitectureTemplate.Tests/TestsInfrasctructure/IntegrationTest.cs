using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.Common.Behaviors;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Domain.Services;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Infrastructure.FileSystemStorage;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Repositories;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Serialization;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public abstract class IntegrationTest : IDisposable
    {
        private readonly ServiceCollection _serviceCollection;
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private readonly Guid _testIdentifier;
        private InMemoryTestDatabase _inMemoryTestDatabase;

        private static IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            var defaultServiceProviderFactory = new DefaultServiceProviderFactory(new ServiceProviderOptions());
            return defaultServiceProviderFactory.CreateServiceProvider(serviceCollection);
        }

        protected TestSideEffects SideEffects { get; private set; }
        protected TestSeeder Seed { get; private set; }

        static IntegrationTest()
        {
            MappingLoader.LoadClassMappings();
            SerializationConventions.SetConventions();
        }

        protected IntegrationTest()
        {
            _testIdentifier = Guid.NewGuid();

            _serviceCollection = new ServiceCollection(); // TODO is it necessary to assign it to a field?
            InstallDependencies(_serviceCollection);
            
            _serviceProvider = new Lazy<IServiceProvider>(() => GetServiceProvider(_serviceCollection));
            
            SideEffects = new TestSideEffects(_testIdentifier, database: null);
            Seed = new TestSeeder(_testIdentifier, database: null);
        }

        private void InstallDependencies(IServiceCollection services)
        {
            services.AddMediatR(typeof(RequestValidationBehavior<,>).Assembly);
            services.AddFluentValidation();
            services.AddRepositories();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddSingleton(provider => new TestMongoDatabaseFactory(_inMemoryTestDatabase).GetDatabase());
            services.AddSingleton<IBookContentRepository, BookContentRepository>();
            services.AddSingleton<IUpdateBookReferenceService, BookMetadataRepository>();
            services.AddScoped<IBookStorageConfiguration>(_serviceProvider => new TestBookStorageConfiguration(_testIdentifier));
        }

        protected void RebuildDatabase()
        {
            _inMemoryTestDatabase = new InMemoryTestDatabase(databaseName: _testIdentifier.ToString());
            _inMemoryTestDatabase.Drop();

            var database = _inMemoryTestDatabase.GetDatabase();
            SideEffects = new TestSideEffects(_testIdentifier, database);
            Seed = new TestSeeder(_testIdentifier, database);
        }

        [DebuggerStepThrough]
        protected Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var scope = _serviceProvider.Value.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return mediator.Send(request, CancellationToken.None);
        }

        private class InMemoryTestDatabase
        {
            public const string LocalhostInMemoryConnectionString = "mongodb://localhost:27018"; // TODO load from configuration
            public string DatabaseName { get; }

            public InMemoryTestDatabase(string databaseName)
            {
                DatabaseName = databaseName;
            }

            public void Drop()
            {
                var mongoClient = GetMongoClient(LocalhostInMemoryConnectionString);
                mongoClient.DropDatabase(DatabaseName);
            }
            public IMongoDatabase GetDatabase()
            {
                var mongoClient = GetMongoClient(LocalhostInMemoryConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(DatabaseName);
                
                MappingLoader.EnsureIndices(mongoDatabase);

                return mongoDatabase;
            }

            private static MongoClient GetMongoClient(string connectionString) => new(connectionString);
        }

        private class TestMongoDatabaseFactory : IMongoDatabaseFactory
        {
            private readonly InMemoryTestDatabase _inMemoryTestDatabase;

            public TestMongoDatabaseFactory(InMemoryTestDatabase inMemoryTestDatabase)
            {
                _inMemoryTestDatabase = inMemoryTestDatabase;
            }

            public IMongoDatabase GetDatabase() => _inMemoryTestDatabase.GetDatabase();
        }

        public void Dispose()
        {
            _inMemoryTestDatabase.Drop();
            CleanFileSystem();
        }

        private void CleanFileSystem()
        {
            var bookStorageLocation = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, _testIdentifier.ToString()));
            if (bookStorageLocation.Exists)
                bookStorageLocation.Delete(recursive: true);
        }
    }
}