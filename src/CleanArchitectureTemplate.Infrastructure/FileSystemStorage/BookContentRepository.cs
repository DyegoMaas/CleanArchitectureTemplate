using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Infrastructure.FileSystemStorage
{
    public class BookContentRepository : IBookContentRepository
    {
        private DirectoryInfo BookStorageLocation { get; }
        
        public BookContentRepository(IBookStorageConfiguration bookStorageConfiguration)
        {
            var booksRelativePath = bookStorageConfiguration.BooksRelativePath;
            BookStorageLocation = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, booksRelativePath));
            if (!BookStorageLocation.Exists)
                BookStorageLocation.Create();
        }

        public async Task<LibraryPath> StoreBookContent(ReadOnlyMemory<byte> bookContent, Guid galacticRegistryId, CancellationToken cancellationToken)
        {
            var bookDirectoryInfo = new DirectoryInfo(Path.Combine(BookStorageLocation.FullName, galacticRegistryId.ToString()));
            bookDirectoryInfo.Create();

            var bookPath = Path.Combine(bookDirectoryInfo.FullName, "full.book");
            await using var fileStream = File.Create(bookPath);
            await fileStream.WriteAsync(bookContent, cancellationToken);
            
            return new LibraryPath(bookPath);
        }
    }
}