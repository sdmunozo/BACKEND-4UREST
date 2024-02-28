namespace NetShip.DTOs.Auth
{
    public class AuthenticationResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
