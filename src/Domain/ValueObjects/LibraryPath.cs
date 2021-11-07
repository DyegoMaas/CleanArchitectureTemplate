namespace Domain.ValueObjects
{
    public class LibraryPath
    {
        public string Path { get; }

        public LibraryPath(string path)
        {
            Path = path;
        }
    }
}