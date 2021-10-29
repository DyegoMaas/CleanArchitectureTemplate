using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Domain.Services;
using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Repositories
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