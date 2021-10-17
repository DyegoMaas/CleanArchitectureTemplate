namespace CleanArchitectureTemplate.Infrastructure.FileSystemStorage
{
    public interface IBookStorageConfiguration
    {
        string BooksRelativePath { get; }
    }
}