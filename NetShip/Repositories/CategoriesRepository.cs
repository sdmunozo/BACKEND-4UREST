using Microsoft.EntityFrameworkCore;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext context;

        public CategoriesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Create(Category category)
        {
            context.Add(category);
            await context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> Exist(int id)
        {
            return await context.Categories.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Category?> GetCategory(int id)
        {
            return await context.Categories.FirstOrDefaultAsync( x => x.Id == id);
        }

        public async Task Update(Category category)
        {
            context.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Categories.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
