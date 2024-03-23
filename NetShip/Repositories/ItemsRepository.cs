using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public ItemsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Item item)
        {
            context.Add(item);
            await context.SaveChangesAsync();
            return item.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Items.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Item>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Items.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Items.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Item?> GetById(Guid id)
        {
            return await context.Items.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Item item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Items.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Item>> GetByName(string name)
        {
            return await context.Items.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        /*
        public async Task SetPlatforms(Guid id, List<PricePerItemPerPlatform> platforms)
        {
            var item = await context.Items
                .Include(i => i.PricePerItemPerPlatforms)
                .FirstOrDefaultAsync(i => i.Id == id);

            if(item is null)
            {
                throw new ArgumentException($"No existe un Item con el Id {id}");
            }

            item.PricePerItemPerPlatforms = mapper.Map(platforms, item.PricePerItemPerPlatforms);

            await context.SaveChangesAsync();
        }

        */

        public async Task<Guid?> GetBranchIdOfItem(Guid itemId)
        {
            var itemWithBranch = await context.Items
                                                 .Where(p => p.Id == itemId)
                                                 .Select(p => p.Category.Catalog.BranchId)
                                                 .FirstOrDefaultAsync();
            return itemWithBranch;
        }

        public async Task<Guid?> GetBrandIdOfItem(Guid itemId)
        {
            var itemWithBrand = await context.Items
                                              .Where(i => i.Id == itemId)
                                              .Select(i => i.Category.Catalog.Branch.BrandId)
                                              .FirstOrDefaultAsync();
            return itemWithBrand;
        }

    }
}