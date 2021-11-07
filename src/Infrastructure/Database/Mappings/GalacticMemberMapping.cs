using Domain.ValueObjects;
using Infrastructure.Database.Common;
using MongoDB.Bson.Serialization;

namespace Infrastructure.Database.Mappings
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