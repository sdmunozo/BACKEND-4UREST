using Microsoft.AspNetCore.Identity;
using NetShip.DTOs.Auth;
using NetShip.Entities;
using System.Security.Cryptography.X509Certificates;

namespace NetShip.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UserServices(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<ApplicationUser> GetUser()
        {
            var idClaim = httpContextAccessor.HttpContext!
                .User.Claims.Where(x => x.Type == "id").FirstOrDefault();

            if (idClaim is null)
            {
                return null;
            }

            var userId = idClaim.Value;
            return await userManager.FindByIdAsync(userId);
        }

    }
}
