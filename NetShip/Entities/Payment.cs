using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetShip.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Currency { get; set; } = "MXN"; // MXN, USD
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Tip { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? TipPercentage { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Method { get; set; } = null!; // cash, card, transfer, other
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // pending, processing, completed, failed, refunded
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ProcessedAt { get; set; }
        
        [StringLength(100)]
        public string? TransactionReference { get; set; }
        
        // Card details (if applicable)
        [StringLength(4)]
        public string? CardLast4 { get; set; }
        
        [StringLength(20)]
        public string? CardBrand { get; set; }
        
        // Cash details (if applicable)
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CashReceived { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CashChange { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Navigation properties
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public Guid? ProcessedByWaiterId { get; set; }
        public Waiter? ProcessedByWaiter { get; set; }
    }
}