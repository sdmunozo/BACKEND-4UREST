using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class LoyaltyTransaction
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = null!; // earn, redeem, expire, adjustment
        
        [Required]
        public int Points { get; set; } // Positive for earn, negative for redeem
        
        [Required]
        public int BalanceBefore { get; set; }
        
        [Required]
        public int BalanceAfter { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Reason { get; set; } = null!; // purchase, bonus, promotion, redemption, expiration, manual_adjustment
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ExpiresAt { get; set; }
        
        // For redemptions
        [StringLength(50)]
        public string? RedemptionCode { get; set; }
        
        [StringLength(100)]
        public string? RewardName { get; set; }
        
        // Navigation properties
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
        
        public Guid? ProcessedById { get; set; }
        public ApplicationUser? ProcessedBy { get; set; }
    }
}