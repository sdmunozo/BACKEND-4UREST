using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class ModifiersGroupsRepository : IModifiersGroupsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public ModifiersGroupsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(ModifiersGroup modifiersGroup)
        {
            context.Add(modifiersGroup);
            await context.SaveChangesAsync();
            return modifiersGroup.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.ModifiersGroups.AnyAsync(x => x.Id == id);
        }

        public async Task<List<ModifiersGroup>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.ModifiersGroups.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.ModifiersGroups.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ModifiersGroup?> GetById(Guid id)
        {
            return await context.ModifiersGroups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(ModifiersGroup modifiersGroup)
        {
            context.Update(modifiersGroup);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.ModifiersGroups.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<ModifiersGroup>> GetByName(string name)
        {
            return await context.ModifiersGroups.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

    }
}