using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Repositories
{
    public class BookMetadataRepository : MongoDbRepository<BookMetadata>, IBookMetadataRepository
    {
        public BookMetadataRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public Task AddBook(BookMetadata bookMetadata)
        {
            return Collection.InsertOneAsync(bookMetadata);
        }
    }
}