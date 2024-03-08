using NetShip.DTOs.Brand;
using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IBrandsRepository
    {
        Task<BrandDTO?> GetFirstBrandByUserId(string userId);
        Task<List<Brand>> GetBrandsByUserId(string userId);
        Task<List<Brand>> GetAll(PaginationDTO paginationDTO);
        Task<Brand?> GetById(Guid id);
        Task<Guid> Create(Brand brand);
        Task<bool> Exist(Guid id);
        Task Update(Brand brand);
        Task Delete(Guid id);
        Task<List<Brand>> GetByName(string name);

        Task<List<Guid>> GetBrandIdsByUserId(string userId);

        Task<Guid?> GetFirstBrandIdByUserId(string userId);

        Task<List<Brand>> GetBrandsWithBranchesByUserId(string userId);
    }
}
