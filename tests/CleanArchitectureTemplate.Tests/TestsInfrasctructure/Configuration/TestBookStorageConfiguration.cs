using System;
using System.IO;
using CleanArchitectureTemplate.Infrastructure.FileSystem;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure.Configuration
{
    internal class TestBookStorageConfiguration : IBookStorageConfiguration
    {
        public Guid TestSessionIdentifier { get; }

        public TestBookStorageConfiguration(Guid testSessionIdentifier)
        {
            TestSessionIdentifier = testSessionIdentifier;
        }

        public string BooksRelativePath => Path.Combine(TestSessionIdentifier.ToString(), "books");
    }
}