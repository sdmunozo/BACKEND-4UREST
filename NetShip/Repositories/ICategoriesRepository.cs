using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetAll(PaginationDTO paginationDTO);
        Task<Category?> GetById(Guid id);
        Task<Guid> Create(Category category);
        Task<bool> Exist(Guid id); 
        Task Update (Category category);
        Task Delete (Guid id);
        Task<List<Category>> GetByName(string name);
        Task<Category?> GetCategoryWithCatalogById(Guid categoryId);
    }
}
