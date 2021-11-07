using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;

namespace Infrastructure.Database.Common
{
    public static class MappingLoader
    {
        private static readonly object _lock = new();
        private static bool MappingsAreAlreadyLoaded;

        private static List<Type> MappingTypes { get; } = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IMongoDbMapping).IsAssignableFrom(t))
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        public static void LoadClassMappings()
        {
            lock (_lock)
            {
                if (MappingsAreAlreadyLoaded)
                    return;
            
                foreach (var mappingType in MappingTypes)
                {
                    var mongoMapping = (IMongoDbMapping)Activator.CreateInstance(mappingType);
                    mongoMapping?.RegisterBsonClassMap();
                }
                MappingsAreAlreadyLoaded = true;
            }
        }
        
        public static void EnsureIndices(IMongoDatabase mongoDatabase)
        {
            foreach (var mappingType in MappingTypes)
            {
                var mongoMapping = (IMongoDbMapping)Activator.CreateInstance(mappingType);
                mongoMapping?.EnsureIndices(mongoDatabase);
            }
        }
    }
}