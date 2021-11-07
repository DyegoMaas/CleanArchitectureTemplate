using MongoDB.Driver;

namespace Infrastructure.Database.Common
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase GetDatabase();
    }
}