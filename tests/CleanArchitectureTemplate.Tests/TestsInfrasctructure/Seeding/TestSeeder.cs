using System;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public class TestSeeder
    {
        private readonly IMongoDatabase _database;

        public TestSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public DatabaseDocumentSeeder DatabaseDocument
        {
            get
            {
                if (_database is null)
                    throw new InvalidOperationException("Database was not initialized. Use `RebuildDatabase()` method to prepare the test database");
                
                return new DatabaseDocumentSeeder(_database);
            }
        }
    }
}