namespace NetShip.DTOs.CatalogDTOs
{
    public class CatalogDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Icon { get; set; }
    }
}
