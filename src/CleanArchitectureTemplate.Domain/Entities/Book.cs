using System;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Domain.Entities
{
    public class Book : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public GalacticMember Origin { get; set; }
        public string Publisher { get; set; }
        public int GalacticYear { get; set; }
        public Guid GalacticRegistryId { get; set; }
        
        public static Book Create(string name, string description, string author, GalacticMember origin, string publisher, int galacticYear)
        {
            return new Book
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