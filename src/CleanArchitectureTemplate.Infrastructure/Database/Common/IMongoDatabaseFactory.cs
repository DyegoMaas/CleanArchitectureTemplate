using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.Database.Common
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase GetDatabase();
    }
}