using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.Database.Common
{
    public abstract class MongoDbValueObjectMapping<TValueObject> : IMongoDbMapping
    {
        public string CollectionName => string.Empty;

        public void EnsureIndices(IMongoDatabase mongoDatabase)
        {
        }

        public void RegisterBsonClassMap()
        {
            BsonClassMap.RegisterClassMap<TValueObject>(classMap =>
            {
                classMap.AutoMap();
                RegisterBsonClassMap(classMap);
                classMap.SetIgnoreExtraElements(true);
            });
        }

        protected abstract void RegisterBsonClassMap(BsonClassMap<TValueObject> classMap);
    }
}