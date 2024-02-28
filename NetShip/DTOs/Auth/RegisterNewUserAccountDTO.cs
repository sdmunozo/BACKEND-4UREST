namespace NetShip.DTOs.Auth
{
    public class RegisterNewUserAccountDTO
    {
        public string UserFirstName { get; set; } = null!;
        public string UserLastName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
    }
}
