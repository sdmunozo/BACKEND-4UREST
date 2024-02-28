using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class BranchesRepository : IBranchesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public BranchesRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Branch branch)
        {
            context.Add(branch);
            await context.SaveChangesAsync();
            return branch.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Branches.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Branch>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Branches.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Branches.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Branch?> GetById(Guid id)
        {
            return await context.Branches.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Branch branch)
        {
            context.Update(branch);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Branches.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Branch>> GetByName(string name)
        {
            return await context.Branches.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }
    }
}
