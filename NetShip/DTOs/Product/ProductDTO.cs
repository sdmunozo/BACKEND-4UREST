namespace NetShip.DTOs.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
