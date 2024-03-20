using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class ModifiersRepository : IModifiersRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public ModifiersRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<Guid> Create(Modifier modifier)
        {
            context.Add(modifier);
            await context.SaveChangesAsync();
            return modifier.Id;
        }

        public async Task<bool> Exist(Guid id)
        {
            return await context.Modifiers.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Modifier>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = context.Modifiers.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await context.Modifiers.Paginate(paginationDTO).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Modifier?> GetById(Guid id)
        {
            return await context.Modifiers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Modifier modifier)
        {
            context.Update(modifier);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await context.Modifiers.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Modifier>> GetByName(string name)
        {
            return await context.Modifiers.Where(c => c.Name.Contains(name)).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task SetPlatforms(Guid id, List<PricePerModifierPerPlatform> platforms)
        {
            var modifier = await context.Modifiers
                .Include(i => i.PricePerModifierPerPlatforms)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (modifier is null)
            {
                throw new ArgumentException($"No existe un Modifier con el Id {id}");
            }

            modifier.PricePerModifierPerPlatforms = mapper.Map(platforms, modifier.PricePerModifierPerPlatforms);

            await context.SaveChangesAsync();
        }

    }
}