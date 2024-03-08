namespace NetShip.Entities
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public Guid BranchId { get; set; }
        public string? Icon { get; set; }
    }
}
