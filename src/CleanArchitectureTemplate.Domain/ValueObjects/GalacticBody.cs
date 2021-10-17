namespace CleanArchitectureTemplate.Domain.ValueObjects
{
    public record GalacticBody
    {
        public string Planet { get; init; }
        public string System { get; init; }

        public GalacticBody(string planet, string system)
        {
            Planet = planet;
            System = system;
        }
    }
}