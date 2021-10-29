using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.Database.Common
{
    public abstract class MongoDbRepository<TDocument> : IMongoDbRepository
    {
        private readonly IMongoDatabase _mongoDatabase;

        private static string CollectionName => CollectionNamesHelper.CollectionNameFor<TDocument>();

        protected IMongoCollection<TDocument> Collection => _mongoDatabase.GetCollection<TDocument>(CollectionName);

        public MongoDbRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }
    }
}