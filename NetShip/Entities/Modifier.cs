namespace NetShip.Entities
{
    public class Modifier
    {
        public Guid Id { get; set; }
        public Guid ModifiersGroupId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public string? Icon { get; set; }
        public int MinModifier { get; set; } = -1;
        public int MaxModifier { get; set; } = -1;
        public decimal? Price { get; set; }
        //public List<PricePerModifierPerPlatform> PricePerModifierPerPlatforms { get; set; } = new List<PricePerModifierPerPlatform>();
        public ModifiersGroup ModifiersGroup { get; set; }
    }
}
