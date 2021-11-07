using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Database.Common.Serialization
{
    public static class SerializationConventions
    {
        public static void SetConventions()
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime), new MongoUtcDateTimeSerializer());
            
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));

            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
        }
    }
}