using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Table
    {
        public Guid Id { get; set; }
        
        [Required]
        public int Number { get; set; }
        
        [Required]
        public int Capacity { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "available"; // available, occupied, reserved, maintenance
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastOccupiedAt { get; set; }
        
        // Navigation properties
        public Guid AreaId { get; set; }
        public Area Area { get; set; } = null!;
        
        public Guid? CurrentSessionId { get; set; }
        public TableSession? CurrentSession { get; set; }
        
        public ICollection<TableSession> Sessions { get; set; } = new List<TableSession>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}