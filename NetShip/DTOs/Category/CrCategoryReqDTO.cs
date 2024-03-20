namespace NetShip.DTOs.Category
{
    public class CrCategoryReqDTO
    {
        public Guid CatalogId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public IFormFile? Icon { get; set; }
    }
}
