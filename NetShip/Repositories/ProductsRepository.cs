using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public ProductsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<Guid> Create(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Products.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Products.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
        }

        public async Task<Product?> GetById(Guid id)
        {
            return await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Products.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Product>> GetByName(string name)
        {
            return await context.Products.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Product?> GetProductWithCategoryById(Guid productId)
        {
            return await context.Products
                                .Include(p => p.Category)
                                .ThenInclude(c => c.Catalog)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Guid?> GetBranchIdOfProduct(Guid productId)
        {
            var productWithBranch = await context.Products
                                                 .Where(p => p.Id == productId)
                                                 .Select(p => p.Category.Catalog.BranchId)
                                                 .FirstOrDefaultAsync();
            return productWithBranch;
        }

    }
}
