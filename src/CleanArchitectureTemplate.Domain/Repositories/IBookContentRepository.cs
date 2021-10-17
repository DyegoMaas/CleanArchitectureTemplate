using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Domain.Repositories
{
    public interface IBookContentRepository : IRepository

    {
        Task<LibraryPath> StoreBookContent(ReadOnlyMemory<byte> bookContent, Guid galacticRegistryId,
            CancellationToken cancellationToken);
    }
}