namespace NetShip.Entities
{
    public class PricePerModifierPerPlatform
    {
        public Guid Id { get; set; }
        public Guid ModifierId { get; set; }
        public Guid PlatformId { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = true;

        //Propiedades de Navegacion:
        public Modifier Modifier { get; set; }
        public Platform Platform { get; set; }
    }
}
