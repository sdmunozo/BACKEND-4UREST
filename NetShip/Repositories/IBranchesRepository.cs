using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IBranchesRepository
    {
        Task<BranchDTO?> GetFirstByBrandId(Guid brandId);
        Task<List<BranchDTO>> GetByBrandId(Guid brandId);
        Task<List<Branch>> GetAll(PaginationDTO paginationDTO);
        Task<Branch?> GetById(Guid id);
        Task<Guid> Create(Branch branch);
        Task<bool> Exist(Guid id);
        Task Update(Branch branch);
        Task Delete(Guid id);
        Task<List<Branch>> GetByName(string name);
        Task<List<Guid>> GetBranchIdsByBrandId(Guid brandId);
        Task<Guid?> GetFirstBranchIdByBrandId(Guid brandId);
        Task<Branch?> GetByUrlNormalizedName(string urlNormalizedName);
        Task<Brand?> GetBrandByBranchId(Guid branchId);
        Task<Branch> GetByNormalizedDigitalMenu(string normalizedDigitalMenu);
        Task<List<Catalog>> GetCatalogsByBranchId(Guid branchId);
         Task<List<Category>> GetCategoriesByCatalogId(Guid catalogId);
         Task<List<Item>> GetItemsByCategoryId(Guid categoryId);
    }
}
