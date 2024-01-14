using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public Guid ParentId { get; set; }
        public string Status { get; set; } = "Activo";
    }
}
