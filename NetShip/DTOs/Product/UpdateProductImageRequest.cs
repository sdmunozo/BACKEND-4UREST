namespace NetShip.DTOs.Product
{
    public class UpdateProductImageRequest
    {
        public Guid ProductId { get; set; }
        public IFormFile Icon { get; set; }
    }

}
