using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category?> GetCategory(int id);
        Task<int> Create(Category category);
        Task<bool> Exist(int id); 
        Task Update (Category category);
        Task Delete (int id);
    }
}
