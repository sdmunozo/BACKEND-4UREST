namespace NetShip.Entities
{
    public class Branch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
