namespace NetShip.DTOs.Platform
{
    public class PlatformDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Icon { get; set; }
        public Guid BrandId { get; set; }
    }
}
