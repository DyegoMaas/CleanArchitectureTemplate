using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Mappings
{
    public class BookMapping : MongoDbEntityMapping<Book>
    {
        public override string CollectionName => "books";
        
        protected override void RegisterBsonClassMap(BsonClassMap<Book> classMap)
        {
            classMap.MapMember(x => x.GalacticRegistryId).SetElementName("galactic_registry_id")
                // .SetSerializer(new GuidSerializer(BsonType.String))
                .SetIsRequired(true);
            classMap.MapMember(x => x.Name).SetElementName("name").SetIsRequired(true);
            classMap.MapMember(x => x.Description).SetElementName("description").SetIsRequired(true);
            classMap.MapMember(x => x.Author).SetElementName("author").SetIsRequired(true);
            classMap.MapMember(x => x.Publisher).SetElementName("publisher").SetIsRequired(true);
            classMap.MapMember(x => x.GalacticYear).SetElementName("galactic_year").SetIsRequired(true);
            classMap.MapMember(x => x.Origin).SetElementName("origin").SetIsRequired(false);
        }

        protected override void SetupIndices(IndicesManager indicesManager)
        {
            indicesManager.AddIndex(IndexBuilder.Ascending(x => x.GalacticRegistryId), unique: true);
        }
    }
}