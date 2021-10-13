using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public interface IMongoDbMapping
    {
        void RegisterBsonClassMap();
        void EnsureIndices(IMongoDatabase mongoDatabase);
    }
}