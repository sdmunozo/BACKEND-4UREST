namespace NetShip.DTOs.CatalogDTOs
{
    public class UpdateCatalogDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
