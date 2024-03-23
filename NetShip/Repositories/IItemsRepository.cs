using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IItemsRepository
    {
        Task<List<Item>> GetAll(PaginationDTO paginationDTO);
        Task<Item?> GetById(Guid id);
        Task<Guid> Create(Item item);
        Task<bool> Exist(Guid id);
        Task Update(Item item);
        Task Delete(Guid id);
        Task<List<Item>> GetByName(string name);
        //Task SetPlatforms(Guid id, List<PricePerItemPerPlatform> platforms);
        Task<Guid?> GetBranchIdOfItem(Guid itemId);
        Task<Guid?> GetBrandIdOfItem(Guid itemId);
    }
}
