using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBookMetadataRepository : IRepository
    {
        public Task AddBook(BookMetadata bookMetadata);
        Task<BookMetadata> GetBookAsync(Guid galacticRegistryId);
    }
}