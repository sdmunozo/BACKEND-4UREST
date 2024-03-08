using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;

namespace NetShip.DTOs.Auth
{
    public class AuthenticationResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public UserResponseDTO User { get; set; }
        public BrandDTO Brand { get; set; }
        public BranchDTO Branch { get; set; }
    }
}
