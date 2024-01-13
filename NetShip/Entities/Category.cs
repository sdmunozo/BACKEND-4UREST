using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
