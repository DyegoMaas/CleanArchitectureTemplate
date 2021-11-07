using MongoDB.Driver;

namespace Infrastructure.Database.Common
{
    public interface IMongoDbMapping
    {
        void RegisterBsonClassMap();
        void EnsureIndices(IMongoDatabase mongoDatabase);
    }
}