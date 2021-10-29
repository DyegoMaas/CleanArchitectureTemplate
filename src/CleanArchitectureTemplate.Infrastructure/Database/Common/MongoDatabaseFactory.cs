using System;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        public IMongoDatabase GetDatabase()
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            return mongoDatabase;
        }
    }
}