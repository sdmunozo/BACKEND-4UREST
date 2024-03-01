using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public BrandsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Brand brand)
        {
            context.Add(brand);
            await context.SaveChangesAsync();
            return brand.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Brands.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Brand>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Brands.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Brands.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Brand?> GetById(Guid id)
        {
            return await context.Brands.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Brand brand)
        {
            context.Update(brand);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Brands.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Brand>> GetByName(string name)
        {
            return await context.Brands.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<List<Guid>> GetBrandIdsByUserId(string userId)
        {
            return await context.Brands
                                .Where(b => b.User.Id == userId)
                                .Select(b => b.Id)
                                .ToListAsync();
        }

        public async Task<Guid?> GetFirstBrandIdByUserId(string userId)
        {
            return await context.Brands
                                .Where(b => b.User.Id == userId)
                                .Select(b => b.Id)
                                .FirstOrDefaultAsync();
        }

        public async Task<List<Brand>> GetBrandsWithBranchesByUserId(string userId)
        {
            return await context.Brands
                                .Where(b => b.User.Id == userId)
                                .Include(b => b.Branches) 
                                .ToListAsync();
        }
    }
}
