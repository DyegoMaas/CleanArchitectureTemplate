using System;
using System.Linq;
using System.Reflection;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common
{
    public static class CollectionNamesHelper
    {
        // TODO implement tests for non entity document types (what should be behavior in these cases?)
        public static string CollectionNameFor<TDocument>()
        {
            var mappingTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAbstract is false)
                .Where(x => x.IsAssignableTo(typeof(IMongoDbMapping)))
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
}