using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Database.Common.Serialization
{
    public class MongoUtcDateTimeSerializer : DateTimeSerializer
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var obj = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, obj);
        }
    }
}