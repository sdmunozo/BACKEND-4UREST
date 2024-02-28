namespace NetShip.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public List<Branch> Branches { get; set; } = new List<Branch>();
    }
}
