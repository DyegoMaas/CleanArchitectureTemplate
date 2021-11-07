using Infrastructure.Database.Common;
using MongoDB.Driver;

namespace Tests.TestsInfrasctructure.Database
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