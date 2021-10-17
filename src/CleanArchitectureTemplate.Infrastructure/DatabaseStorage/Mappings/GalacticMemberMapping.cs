using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Bson.Serialization;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Mappings
{
    public class GalacticMemberMapping : MongoDbValueObjectMapping<GalacticMember>
    {
        protected override void RegisterBsonClassMap(BsonClassMap<GalacticMember> classMap)
        {
            classMap.MapMember(x => x.Planet).SetElementName("planet").SetIsRequired(true);
            classMap.MapMember(x => x.System).SetElementName("system").SetIsRequired(true);
        }
    }
}