using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Bson.Serialization;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Mappings
{
    public class EntityEntityMapping : MongoDbEntityMapping<Entity>
    {
        public override string CollectionName => string.Empty;
        protected override void RegisterBsonClassMap(BsonClassMap<Entity> classMap)
        {
        }
    }
}