namespace NetShip.DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
    }
}
