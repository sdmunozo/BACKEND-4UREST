using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetShip.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // pending, preparing, ready, delivered, cancelled
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? PreparedAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        // Product Name in multiple languages (JSON)
        public string ProductNameAlias { get; set; } = null!; // JSON: {"es": "Café", "en": "Coffee"}
        
        // Navigation properties
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        // Can be either Product or Item
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        
        public Guid? ItemId { get; set; }
        public Item? Item { get; set; }
        
        public ICollection<OrderItemModifier> Modifiers { get; set; } = new List<OrderItemModifier>();
    }
}