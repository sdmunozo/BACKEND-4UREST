using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public CategoriesRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Category category)
        {
            context.Add(category);
            await context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Categories.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Categories.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Categories.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Category category)
        {
            context.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Categories.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Category>> GetByName(string name)
        {
            return await context.Categories.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category?> GetCategoryWithCatalogById(Guid categoryId)
        {
            return await context.Categories
                                 .Include(c => c.Catalog)
                                 .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

    }
}