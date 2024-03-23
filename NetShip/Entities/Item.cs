namespace NetShip.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public Guid CategoryId { get; set; }
        public string? Icon { get; set; }
        public decimal? Price { get; set; }
        //public List<PricePerItemPerPlatform> PricePerItemPerPlatforms { get; set; } = new List<PricePerItemPerPlatform>();
        public Category Category { get; set; }
    }
}
