using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class TableSession
    {
        public Guid Id { get; set; }
        
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        
        public DateTime? EndTime { get; set; }
        
        public int? GuestCount { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active"; // active, completed, cancelled
        
        public decimal? TotalRevenue { get; set; }
        
        // Navigation properties
        public Guid TableId { get; set; }
        public Table Table { get; set; } = null!;
        
        public Guid WaiterId { get; set; }
        public Waiter Waiter { get; set; } = null!;
        
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}