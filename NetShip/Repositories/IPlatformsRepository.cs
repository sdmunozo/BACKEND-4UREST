using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IPlatformsRepository
    {
        Task<List<Platform>> GetAll(PaginationDTO paginationDTO);
        Task<Platform?> GetById(Guid id);
        Task<Guid> Create(Platform platform);
        Task<bool> Exist(Guid id);
        Task Update(Platform platform);
        Task Delete(Guid id);
        Task<List<Platform>> GetByName(string name);
        Task<List<Guid>> CheckAll(List<Guid> ids);
        Task<Guid?> GetBasePlatformIdByBrandId(Guid brandId);
    }
}
