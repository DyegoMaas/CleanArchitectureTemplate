using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public class TestMongoDatabaseFactory : IMongoDatabaseFactory
    {
        private readonly InMemoryTestDatabase _inMemoryTestDatabase;

        public TestMongoDatabaseFactory(InMemoryTestDatabase inMemoryTestDatabase)
        {
            _inMemoryTestDatabase = inMemoryTestDatabase;
        }

        public IMongoDatabase GetDatabase() => _inMemoryTestDatabase.GetDatabase();
    }
}