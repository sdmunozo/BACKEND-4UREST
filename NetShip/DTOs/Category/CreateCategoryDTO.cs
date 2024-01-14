namespace NetShip.DTOs.Category
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public Guid ParentId { get; set; }
        public string Status { get; set; } = "Activo";
    }
}
