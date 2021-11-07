using System;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class BookMetadata : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public GalacticMember Origin { get; set; }
        public string Publisher { get; set; }
        public int GalacticYear { get; set; }
        public Guid GalacticRegistryId { get; set; }
        public string ContentLocation { get; set; }

        public static BookMetadata Create(string name, string description, string author, GalacticMember origin, string publisher, int galacticYear)
        {
            return new BookMetadata
            {
                Name = name,
                Description = description,
                Author = author,
                Origin = origin,
                Publisher = publisher,
                GalacticYear = galacticYear,
                GalacticRegistryId = Guid.NewGuid() // TODO this ID should be created by an external generator
            };
        }
    }
}