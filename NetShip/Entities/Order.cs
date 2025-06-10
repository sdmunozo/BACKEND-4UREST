using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetShip.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        
        [Required]
        public int OrderNumber { get; set; } // Sequential number per day
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "draft"; // draft, sent, preparing, ready, delivered, paid, cancelled
        
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = "dine-in"; // dine-in, takeout
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; } = 0;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; } = 0;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceCharge { get; set; } = 0;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; } = 0;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [StringLength(500)]
        public string? KitchenNotes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? SentToKitchenAt { get; set; }
        
        public DateTime? ReadyAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        public DateTime? PaidAt { get; set; }
        
        public DateTime? CancelledAt { get; set; }
        
        public int? KitchenTicketNumber { get; set; }
        
        [StringLength(20)]
        public string Priority { get; set; } = "normal"; // normal, urgent
        
        // Navigation properties
        public Guid TableId { get; set; }
        public Table Table { get; set; } = null!;
        
        public Guid WaiterId { get; set; }
        public Waiter Waiter { get; set; } = null!;
        
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        
        public Guid? SessionId { get; set; }
        public TableSession? Session { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}