namespace NetShip.DTOs.ModifiersGroup
{
    public class CrModifiersGroupReqDTO
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Alias { get; set; }
        public IFormFile? Icon { get; set; }
        public int MinModifiers { get; set; }
        public int MaxModifiers { get; set; }
    }
}
