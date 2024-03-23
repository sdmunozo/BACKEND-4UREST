namespace NetShip.DTOs.PricePerItemPerPlatformDTOs
{
    public class PricePerItemPerPlatformDTO
    {
        public Guid PlatformId { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
