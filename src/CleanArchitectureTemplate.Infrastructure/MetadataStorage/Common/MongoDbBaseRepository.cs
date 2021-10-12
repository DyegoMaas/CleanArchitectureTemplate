using System;
using System.Linq;
using CleanArchitectureTemplate.Domain.Entities;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public abstract class MongoDbBaseRepository<TDocument>
    {
        private readonly IMongoDatabase _mongoDatabase;

        private static string CollectionName => CollectionNamesHelper.CollectionNameFor<TDocument>();

        protected IMongoCollection<TDocument> Collection => _mongoDatabase.GetCollection<TDocument>(CollectionName);

        public MongoDbBaseRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }
    }
    
    public static class CollectionNamesHelper
    {
        public static string CollectionNameFor<TDocument>()
        {
            var baseMappingType = typeof(MongoDbMapping<>);
            var mappingTypes = baseMappingType.Assembly
                .GetTypes()
                .Where(x => x.IsAbstract is false)
                .Where(x => x.IsAssignableTo(baseMappingType))
                .ToList();
            if (!mappingTypes.Any())
            {
                throw new InvalidOperationException($"There is no MongoDB mapping for entity {typeof(TDocument).Name}");
            }
            
            var mappingType = mappingTypes.First();
            var propertyInfo = mappingType.GetProperty("CollectionName");
            if (propertyInfo == null)
                throw new InvalidOperationException($"Could not retrieve collection name: {typeof(TDocument).Name}");

            var instance = Activator.CreateInstance(mappingType);
            return (string)propertyInfo.GetValue(instance);
        }
    }

    public abstract class MongoDbMapping<TDocument>
    {
        public abstract string CollectionName { get; }
    }

    public class BookMapping : MongoDbMapping<Book>
    {
        public override string CollectionName => "books";
    }
}