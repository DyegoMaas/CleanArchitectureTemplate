using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Common.Behaviors;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Infrastructure.Database.Common;
using CleanArchitectureTemplate.Infrastructure.Database.Common.Serialization;
using CleanArchitectureTemplate.Infrastructure.Database.Repositories;
using CleanArchitectureTemplate.Infrastructure.FileSystem;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure.Configuration;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure.Database;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure.Seeding;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure.SideEffects;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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