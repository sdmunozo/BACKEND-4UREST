using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class ServiceRequest
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; } = null!; // call_waiter, request_bill, water, utensils, napkins, clean_table, etc.
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // pending, in_progress, completed, cancelled
        
        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "medium"; // low, medium, high
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? RespondedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public int? EstimatedResponseTime { get; set; } // in minutes
        
        // Navigation properties
        public Guid TableId { get; set; }
        public Table Table { get; set; } = null!;
        
        public Guid? WaiterId { get; set; }
        public Waiter? Waiter { get; set; }
        
        public Guid? RespondedByWaiterId { get; set; }
        public Waiter? RespondedByWaiter { get; set; }
    }
}