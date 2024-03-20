namespace NetShip.DTOs.Platform
{
    public class UpPlatformReqDTO
    {
        public Guid BrandId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
