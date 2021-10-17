using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Domain.Repositories
{
    public interface IBookMetadataRepository
    {
        public Task AddBook(BookMetadata bookMetadata);
        Task<BookMetadata> GetBookAsync(Guid galacticRegistryId);
    }
}