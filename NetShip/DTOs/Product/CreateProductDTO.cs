namespace NetShip.DTOs.Product
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public IFormFile? Icon { get; set; }
        public Guid CategoryId { get; set; }
        public string Status { get; set; } = "Activo";
    }
}
