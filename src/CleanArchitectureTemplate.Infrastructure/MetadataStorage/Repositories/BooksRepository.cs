using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Infrastructure.MetadataStorage.Common;
using MongoDB.Driver;

namespace CleanArchitectureTemplate.Infrastructure.MetadataStorage.Repositories
{
    public class BooksRepository : MongoDbBaseRepository<Book>, IBooksRepository
    {
        public BooksRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public Task AddBook(Book book)
        {
            return Collection.InsertOneAsync(book);
        }
    }
}