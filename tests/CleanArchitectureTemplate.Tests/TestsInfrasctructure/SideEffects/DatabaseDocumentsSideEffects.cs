using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitectureTemplate.Infrastructure.Database.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public class DatabaseDocumentsSideEffects
    {
        private readonly IMongoDatabase _database;

        public DatabaseDocumentsSideEffects(IMongoDatabase database)
        {
            _database = database;
        }
        
        public IEnumerable<TDocument> GetDocuments<TDocument>()
        {
            var collection = GetCollectionFor<TDocument>();
            return collection.Find(x => true).ToEnumerable();
        }

        public TDocument GetDocument<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            var collection = GetCollectionFor<TDocument>();
            return collection.Find(filter).FirstOrDefault();
        }

        private IMongoCollection<TDocument> GetCollectionFor<TDocument>()
        {
            var collectionName = CollectionNamesHelper.CollectionNameFor<TDocument>();
            return _database.GetCollection<TDocument>(collectionName);
        }
    }
}