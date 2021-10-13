using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Serialization
{
    public static class SerializationConventions
    {
        public static void SetConventions()
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime), new MongoUtcDateTimeSerializer());
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
        }
    }
}