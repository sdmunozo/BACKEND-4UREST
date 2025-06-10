using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = null!;
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastVisit { get; set; }
        
        public int TotalOrders { get; set; } = 0;
        
        public decimal TotalSpent { get; set; } = 0;
        
        public int LoyaltyPoints { get; set; } = 0;
        
        [StringLength(10)]
        public string PreferredLanguage { get; set; } = "es";
        
        // JSON column for preferences
        public string? Preferences { get; set; } // Will store JSON with allergies, dietary restrictions, etc.
        
        // Navigation properties
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();
    }
}