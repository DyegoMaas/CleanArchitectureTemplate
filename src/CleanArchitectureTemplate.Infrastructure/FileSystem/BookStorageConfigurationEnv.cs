using System;

namespace CleanArchitectureTemplate.Infrastructure.FileSystem
{
    public class BookStorageConfigurationEnv : IBookStorageConfiguration
    {
        public string BooksRelativePath => Environment.GetEnvironmentVariable("BOOKS_RELATIVE_PATH");
    }
}