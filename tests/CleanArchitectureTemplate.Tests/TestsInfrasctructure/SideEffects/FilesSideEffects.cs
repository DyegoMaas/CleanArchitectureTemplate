using System;
using System.IO;

namespace CleanArchitectureTemplate.Tests.TestsInfrasctructure
{
    public class FilesSideEffects
    {
        private Guid _testIdentifier;

        public FilesSideEffects(Guid testIdentifier)
        {
            _testIdentifier = testIdentifier;
        }

        public byte[] LoadFileAsBinary(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}