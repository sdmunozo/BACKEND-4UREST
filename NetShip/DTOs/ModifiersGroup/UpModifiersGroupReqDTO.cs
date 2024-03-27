namespace NetShip.DTOs.ModifiersGroup
{
    public class UpModifiersGroupReqDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public string Alias { get; set; } = string.Empty;
        public IFormFile? Icon { get; set; }
        public bool isSelectable { get; set; } = false;
        public int MinModifiers { get; set; } = -1;
        public int MaxModifiers { get; set; } = -1;
    }
}
