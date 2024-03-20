namespace NetShip.Entities
{
    public class Product
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
        public List<ModifiersGroup> ModifiersGroups { get; set; } = new List<ModifiersGroup>();
        public Category Category { get; set; }

    }
}
