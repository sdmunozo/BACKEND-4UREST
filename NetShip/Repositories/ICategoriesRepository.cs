using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category?> GetCategory(Guid id);
        Task<Guid> Create(Category category);
        Task<bool> Exist(Guid id); 
        Task Update (Category category);
        Task Delete (Guid id);
    }
}
