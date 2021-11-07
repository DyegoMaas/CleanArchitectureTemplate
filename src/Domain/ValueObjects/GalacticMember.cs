namespace Domain.ValueObjects
{
    public record GalacticMember
    {
        public string Planet { get; init; }
        public string System { get; init; }

        public GalacticMember(string planet, string system)
        {
            Planet = planet;
            System = system;
        }
    }
}