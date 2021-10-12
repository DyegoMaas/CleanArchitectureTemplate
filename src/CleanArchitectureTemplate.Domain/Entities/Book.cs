using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Domain.Entities
{
    public class Book
    {
        public string Name { get; }
        public string Description { get; }
        public string Author { get; }
        public GalacticBody Origin { get; }
        public string Publisher { get; }
        public int GalacticYear { get; }

        public Book(string name, string description, string author, GalacticBody origin, string publisher, int galacticYear)
        {
            Name = name;
            Description = description;
            Author = author;
            Origin = origin;
            Publisher = publisher;
            GalacticYear = galacticYear;
        }
    }
}