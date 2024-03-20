namespace NetShip.Entities
{
    public class PricePerItemPerPlatform
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid PlatformId { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = true;

        //Propiedades de Navegacion:
        public Item Item { get; set; }
        public Platform Platform { get; set; }

    }
}