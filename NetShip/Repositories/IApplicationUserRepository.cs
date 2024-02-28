using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<string> Create(ApplicationUser user, string password);
        Task Delete(string id);
        Task<bool> Exist(string id);
        Task<List<ApplicationUser>> GetAll(PaginationDTO paginationDTO);
        Task<ApplicationUser?> GetById(string id);
        Task<List<ApplicationUser>> GetByName(string name);
        Task Update(ApplicationUser user);
    }
}