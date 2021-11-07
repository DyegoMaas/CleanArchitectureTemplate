using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Infrastructure.Database.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.Database.Repositories
{
    public class BookMetadataRepository : MongoDbRepository<BookMetadata>, IBookMetadataRepository, IUpdateBookReferenceService
    {
        public BookMetadataRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public Task AddBook(BookMetadata bookMetadata)
        {
            return Collection.InsertOneAsync(bookMetadata);
        }

        public Task<BookMetadata> GetBookAsync(Guid galacticRegistryId)
        {
            return Collection.Find(x => x.GalacticRegistryId == galacticRegistryId).FirstOrDefaultAsync();
        }

        public Task UpdateBookContentReference(Guid galacticRegistryId, LibraryPath libraryPath)
        {
            var updateDefinition = Builders<BookMetadata>.Update
                .Set(x => x.ContentLocation, libraryPath.Path);
            return Collection.FindOneAndUpdateAsync(x => x.GalacticRegistryId == galacticRegistryId, updateDefinition);
        }
    }
}