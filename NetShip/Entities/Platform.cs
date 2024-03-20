namespace NetShip.Entities
{
    public class Platform
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Icon { get; set; }
        public Guid BrandId { get; set; }

        public List<PricePerModifierPerPlatform> PricePerModifierPerPlatforms { get; set; } = new List<PricePerModifierPerPlatform>();
        public List<PricePerItemPerPlatform> PricePerItemPerPlatforms { get; set; } = new List<PricePerItemPerPlatform>();
        public Brand Brand { get; set; }


    }
}
