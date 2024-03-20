namespace NetShip.DTOs.Item
{
    public class ItemDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public Guid CategoryId { get; set; }
        public string? Icon { get; set; }
    }
}
