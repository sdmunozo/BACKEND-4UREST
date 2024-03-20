namespace NetShip.DTOs.Items
{
    public class SetPlatformItem
    {
        public Guid PlatformId { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = false;
    }
}