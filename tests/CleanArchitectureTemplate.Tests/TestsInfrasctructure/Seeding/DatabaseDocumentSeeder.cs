using Infrastructure.Database.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure.Seeding
{
    public class DatabaseDocumentSeeder
    {
        private readonly IMongoDatabase _database;

        public DatabaseDocumentSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public void InsertDocument<TDocument>(TDocument document) 
        {
            var collectionName = CollectionNamesHelper.CollectionNameFor<TDocument>();
            _database.GetCollection<TDocument>(collectionName).InsertOne(document);
        }
    }
}