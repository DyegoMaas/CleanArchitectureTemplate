using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Infrastructure.Database.Common;
using MongoDB.Bson.Serialization;

namespace CleanArchitectureTemplate.Infrastructure.Database.Mappings
{
    public class BookMetadataMapping : MongoDbEntityMapping<BookMetadata>
    {
        public override string CollectionName => "books";
        
        protected override void RegisterBsonClassMap(BsonClassMap<BookMetadata> classMap)
        {
            classMap.MapMember(x => x.GalacticRegistryId).SetElementName("galactic_registry_id")
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