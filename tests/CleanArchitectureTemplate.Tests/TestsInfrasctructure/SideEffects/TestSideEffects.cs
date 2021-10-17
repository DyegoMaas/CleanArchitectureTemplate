using System;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public class TestSideEffects
    {
        private readonly IMongoDatabase _database;

        public TestSideEffects(IMongoDatabase database)
        {
            _database = database;
            OverFileSystem = new FilesSideEffects();
        }

        public FilesSideEffects OverFileSystem { get; }

        public DatabaseDocumentsSideEffects OverDatabaseDocuments
        {
            get
            {
                if (_database is null)
                    throw new InvalidOperationException("Database was not initialized. Use `RebuildDatabase()` method to prepare the test database");
                
                return new DatabaseDocumentsSideEffects(_database);
            }
        }
    }
}