using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IModifiersRepository
    {
        Task<List<Modifier>> GetAll(PaginationDTO paginationDTO);
        Task<Modifier?> GetById(Guid id);
        Task<Guid> Create(Modifier modifier);
        Task<bool> Exist(Guid id);
        Task Update(Modifier modifier);
        Task Delete(Guid id);
        Task<List<Modifier>> GetByName(string name);
        Task SetPlatforms(Guid id, List<PricePerModifierPerPlatform> platforms);
    }
}
