using Microsoft.AspNetCore.Identity;

namespace NetShip.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? DisableTutorial { get; set; }
        public string? ReferralLink { get; set; }
        public string? ReferredBy { get; set; }
        public List<Brand> Brands { get; set; } = new List<Brand>();
    }
}
