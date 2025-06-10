using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Waiter
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Pin { get; set; } = null!;
        
        [StringLength(50)]
        public string? Area { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        
        public ICollection<TableSession> TableSessions { get; set; } = new List<TableSession>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}