using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase GetDatabase();
    }
}