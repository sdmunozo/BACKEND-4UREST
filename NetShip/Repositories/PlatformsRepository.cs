using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class PlatformsRepository : IPlatformsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public PlatformsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Platform platform)
        {
            context.Add(platform);
            await context.SaveChangesAsync();
            return platform.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Platforms.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Platform>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Platforms.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Platforms.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Platform?> GetById(Guid id)
        {
            return await context.Platforms.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Platform platform)
        {
            context.Update(platform);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Platforms.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Platform>> GetByName(string name)
        {
            return await context.Platforms.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<List<Guid>> CheckAll(List<Guid> ids)
        {
            return await context.Platforms.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToListAsync();
        }

    }
}