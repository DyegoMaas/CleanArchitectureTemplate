using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        private const string DatabaseName = "clean-architecture-template";

        public IMongoDatabase GetDatabase()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27018");
            var mongoDatabase = mongoClient.GetDatabase(DatabaseName);
            // TODO configure database
            return mongoDatabase;
        }
    }
}