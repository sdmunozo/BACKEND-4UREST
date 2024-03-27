namespace NetShip.DTOs.Product
{
    public class UpProductReqDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public IFormFile? Icon { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
