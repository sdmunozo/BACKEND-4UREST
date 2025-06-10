using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetShip.Entities
{
    public class OrderItemModifier
    {
        public Guid Id { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        // Modifier Name in multiple languages (JSON)
        public string ModifierNameAlias { get; set; } = null!; // JSON: {"es": "Grande", "en": "Large"}
        
        // Navigation properties
        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        
        public Guid ModifierId { get; set; }
        public Modifier Modifier { get; set; } = null!;
    }
}