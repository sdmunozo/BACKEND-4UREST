namespace NetShip.DTOs.Product
{
    public class UpProductReqDTO
    {
        public Guid CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
