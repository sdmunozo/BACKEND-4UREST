using Microsoft.EntityFrameworkCore;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext context;

        public ProductsRepository(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task<List<Product>> GetProducts()
        {
            return await context.Products.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Product?> GetProduct(Guid id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.Id == id);
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
    }
}
