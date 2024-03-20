namespace NetShip.DTOs.ModifiersGroup
{
    public class ModifiersGroupDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Alias { get; set; }
        public string? Icon { get; set; }
        public int MinModifiers { get; set; }
        public int MaxModifiers { get; set; }
    }
}
