namespace NetShip.DTOs.Modifier
{
    public class SetPlatformModifier
    {
        public Guid PlatformId { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
