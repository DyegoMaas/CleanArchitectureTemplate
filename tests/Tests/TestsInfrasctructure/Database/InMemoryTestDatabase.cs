using Infrastructure.Database.Common;
using MongoDB.Driver;

namespace Tests.TestsInfrasctructure.Database
{
    public class InMemoryTestDatabase
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
}