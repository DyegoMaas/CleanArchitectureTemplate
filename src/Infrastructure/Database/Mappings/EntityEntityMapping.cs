using Domain.Entities;
using Infrastructure.Database.Common;
using MongoDB.Bson.Serialization;

namespace Infrastructure.Database.Mappings
{
    public class EntityEntityMapping : MongoDbEntityMapping<Entity>
    {
        public override string CollectionName => string.Empty;
        protected override void RegisterBsonClassMap(BsonClassMap<Entity> classMap)
        {
        }
    }
}