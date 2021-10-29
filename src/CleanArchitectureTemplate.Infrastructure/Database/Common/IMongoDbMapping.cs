using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.Database.Common
{
    public interface IMongoDbMapping
    {
        void RegisterBsonClassMap();
        void EnsureIndices(IMongoDatabase mongoDatabase);
    }
}