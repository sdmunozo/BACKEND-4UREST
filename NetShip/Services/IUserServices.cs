using NetShip.Entities;

namespace NetShip.Services
{
    public interface IUserServices
    {
        Task<ApplicationUser> GetUser();
    }
}