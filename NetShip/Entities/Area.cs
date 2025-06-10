using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Area
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        
        public ICollection<Table> Tables { get; set; } = new List<Table>();
    }
}