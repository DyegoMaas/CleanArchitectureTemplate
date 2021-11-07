using System;

namespace Infrastructure.FileSystem
{
    public class BookStorageConfigurationEnv : IBookStorageConfiguration
    {
        public string BooksRelativePath => Environment.GetEnvironmentVariable("BOOKS_RELATIVE_PATH");
    }
}