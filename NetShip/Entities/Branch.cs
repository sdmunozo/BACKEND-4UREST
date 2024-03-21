namespace NetShip.Entities
{
    public class Branch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string UrlNormalizedName { get; set; } = null!;
        public string? NormalizedDigitalMenu { get; set; } = null;
        public string? DigitalMenuLink { get; set; } = null;
        public string? QrCodePath { get; set; } = null;
        public Guid BrandId { get; set; }
        public List<Catalog> Catalogs { get; set; } = new List<Catalog>();
        public string DigitalMenuJson { get; set; } = "{}";

        //Propiedades de Navegacion:
        public Brand Brand { get; set; }
    }
}
