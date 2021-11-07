﻿using CleanArchitectureTemplate.Infrastructure.Database.Common;
using Domain.Entities;
using MongoDB.Bson.Serialization;

namespace CleanArchitectureTemplate.Infrastructure.Database.Mappings
{
    public class EntityEntityMapping : MongoDbEntityMapping<Entity>
    {
        public override string CollectionName => string.Empty;
        protected override void RegisterBsonClassMap(BsonClassMap<Entity> classMap)
        {
        }
    }
}