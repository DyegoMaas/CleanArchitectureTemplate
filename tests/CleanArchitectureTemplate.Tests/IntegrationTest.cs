using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests
{
    public abstract class IntegrationTest
    {
        private IMongoDatabase _database;
        private readonly ServiceCollection _serviceCollection;
        private readonly Lazy<IServiceProvider> _serviceProvider;

        private static IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            var defaultServiceProviderFactory = new DefaultServiceProviderFactory(new ServiceProviderOptions());
            return defaultServiceProviderFactory.CreateServiceProvider(serviceCollection);
        }

        protected TestSideEffects SideEffects { get; private set; }

        protected IntegrationTest()
        {
            _serviceCollection = new ServiceCollection();
            InstallDependencies(_serviceCollection);
            
            _serviceProvider = new Lazy<IServiceProvider>(() => GetServiceProvider(_serviceCollection));
        }

        private static void InstallDependencies(ServiceCollection serviceCollection)
        {
        }

        protected void RebuildDatabase()
        {
            var inMemoryTestDatabase = new InMemoryTestDatabase(databaseName: Guid.NewGuid().ToString());
            inMemoryTestDatabase.Drop();
            _database = inMemoryTestDatabase.GetDatabase();

            SideEffects = new TestSideEffects(_database);
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
            private const string LocalhostInMemoryConnectionString = "mongodb://localhost:27018"; // TODO load from configuration
            private readonly string _databaseName;

            public InMemoryTestDatabase(string databaseName)
            {
                _databaseName = databaseName;
            }

            public void Drop()
            {
                var mongoClient = GetMongoClient(LocalhostInMemoryConnectionString);
                mongoClient.DropDatabase(_databaseName);
            }
            public IMongoDatabase GetDatabase()
            {
                var mongoClient = GetMongoClient(LocalhostInMemoryConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(_databaseName);

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

            public IEnumerable<T> GetDocuments<T>()
            {
                var collection = _database.GetCollection<T>(typeof(T).Name); // TODO resolve to the correct name
                return collection.Find(x => true).ToEnumerable();
            }
        }
    }
}