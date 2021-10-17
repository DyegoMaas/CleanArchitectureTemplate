using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.Common.Behaviors;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Serialization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests
{
    public abstract class IntegrationTest: IDisposable
    {
        private readonly ServiceCollection _serviceCollection;
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private InMemoryTestDatabase _inMemoryTestDatabase;

        private static IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            var defaultServiceProviderFactory = new DefaultServiceProviderFactory(new ServiceProviderOptions());
            return defaultServiceProviderFactory.CreateServiceProvider(serviceCollection);
        }

        protected TestSideEffects SideEffects { get; private set; }

        static IntegrationTest()
        {
            MappingLoader.LoadClassMappings();
            SerializationConventions.SetConventions();
        }

        protected IntegrationTest()
        {
            _serviceCollection = new ServiceCollection();
            InstallDependencies(_serviceCollection);
            
            _serviceProvider = new Lazy<IServiceProvider>(() => GetServiceProvider(_serviceCollection));
        }

        private void InstallDependencies(IServiceCollection services)
        {
            services.AddMediatR(typeof(RequestValidationBehavior<,>).Assembly);
            services.AddFluentValidation();
            services.AddRepositories();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddSingleton(provider => new TestMongoDatabaseFactory(_inMemoryTestDatabase).GetDatabase());
        }

        protected void RebuildDatabase()
        {
            _inMemoryTestDatabase = new InMemoryTestDatabase(databaseName: Guid.NewGuid().ToString());
            _inMemoryTestDatabase.Drop();

            var database = _inMemoryTestDatabase.GetDatabase();
            SideEffects = new TestSideEffects(database);
        }
        
        [DebuggerStepThrough]
        protected Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var scope = _serviceProvider.Value.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return mediator!.Send(request, CancellationToken.None);
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

        protected class TestSideEffects
        {
            private readonly IMongoDatabase _database;

            public TestSideEffects(IMongoDatabase database)
            {
                _database = database;
            }

            public IEnumerable<TDocument> GetDocuments<TDocument>()
            {
                var collection = GetCollectionFor<TDocument>();
                return collection.Find(x => true).ToEnumerable();
            }

            public TDocument GetDocument<TDocument>(Expression<Func<TDocument, bool>> filter)
            {
                var collection = GetCollectionFor<TDocument>();
                return collection.Find(filter).FirstOrDefault();
            }

            private IMongoCollection<TDocument> GetCollectionFor<TDocument>()
            {
                var collectionName = CollectionNamesHelper.CollectionNameFor<TDocument>();
                return _database.GetCollection<TDocument>(collectionName);
            }
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
        }
    }
}