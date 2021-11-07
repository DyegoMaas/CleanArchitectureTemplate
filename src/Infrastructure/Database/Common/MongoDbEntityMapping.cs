using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Infrastructure.Database.Common
{
    public abstract class MongoDbEntityMapping<TDocument> : IMongoDbMapping
    {
        public abstract string CollectionName { get; }

        public void EnsureIndices(IMongoDatabase mongoDatabase)
        {
            var indexesManager = new IndicesManager();
            SetupIndices(indexesManager);

            var indexModels = indexesManager
                .GetIndicesDefinition()
                .Select(indexDefinition =>
                    new CreateIndexModel<TDocument>(
                        indexDefinition.Key,
                        new CreateIndexOptions<TDocument> { Unique = indexDefinition.Unique }))
                .ToList();

            if (indexModels.Any())
                mongoDatabase
                    .GetCollection<TDocument>(CollectionName)
                    .Indexes
                    .CreateMany(indexModels);
        }

        public void RegisterBsonClassMap()
        {
            BsonClassMap.RegisterClassMap<TDocument>(bsonClassMap =>
            {
                bsonClassMap.AutoMap();
                RegisterBsonClassMap(bsonClassMap);
                bsonClassMap.SetIgnoreExtraElements(true);
            });
        }

        protected IndexKeysDefinitionBuilder<TDocument> IndexBuilder => Builders<TDocument>.IndexKeys;

        protected abstract void RegisterBsonClassMap(BsonClassMap<TDocument> bsonClassMap);

        protected virtual void SetupIndices(IndicesManager indicesManager)
        {
        }

        protected class IndicesManager
        {
            private readonly IList<(IndexKeysDefinition<TDocument>, bool)> _indices =
                new List<(IndexKeysDefinition<TDocument>, bool)>();

            public IndicesManager AddIndex(IndexKeysDefinition<TDocument> key, bool unique = false)
            {
                _indices.Add((key, unique));
                return this;
            }

            public IEnumerable<(IndexKeysDefinition<TDocument> Key, bool Unique)> GetIndicesDefinition()
            {
                return _indices;
            }
        }
    }
}