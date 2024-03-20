namespace NetShip.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? UrlNormalizedName { get; set; }
        public string? Logo { get; set; } = null!;
        public string? Slogan { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Website { get; set; }
        public string? CatalogsBackground { get; set; } = null!;
        public ApplicationUser User { get; set; }
        public List<Branch> Branches { get; set; } = new List<Branch>();
        public List<Platform> Platforms { get; set; } = new List<Platform>();
        
    }
}
