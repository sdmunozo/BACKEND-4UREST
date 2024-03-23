using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
using NetShip.DTOs.Common;
using NetShip.DTOs.DigitalMenu;
using NetShip.Entities;
using NetShip.Utilities;
using Newtonsoft.Json;

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

        public async Task<List<BranchDTO>> GetByBrandId(Guid brandId)
        {
            return await context.Branches
                                .Where(branch => branch.BrandId == brandId)
                                .Select(branch => new BranchDTO
                                {
                                    Id = branch.Id,
                                    Name = branch.Name
                                })
                                .ToListAsync();
        }


        public async Task<Guid> Create(Branch branch)
        {
            string normalizedName = StringUtils.NormalizeUrlName(branch.Name);
            branch.UrlNormalizedName = normalizedName;
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

        public async Task<List<Guid>> GetBranchIdsByBrandId(Guid brandId)
        {
            return await context.Branches
                                .Where(branch => branch.BrandId == brandId)
                                .Select(branch => branch.Id)
                                .ToListAsync();
        }

        public async Task<Guid?> GetFirstBranchIdByBrandId(Guid brandId)
        {
            return await context.Branches
                                .Where(branch => branch.BrandId == brandId)
                                .Select(branch => branch.Id)
                                .FirstOrDefaultAsync();
        }

        public async Task<BranchDTO?> GetFirstByBrandId(Guid brandId)
        {
            var branch = await context.Branches
                                      .Where(b => b.BrandId == brandId)
                                      .Select(b => new BranchDTO
                                      {
                                          Id = b.Id,
                                          Name = b.Name
                                      })
                                      .FirstOrDefaultAsync();
            return branch;
        }

        public async Task<Branch?> GetByUrlNormalizedName(string urlNormalizedName)
        {
            var branch = await context.Branches
                                      .Where(b => b.UrlNormalizedName == urlNormalizedName)
                                      .FirstOrDefaultAsync();

            return branch;
        }

        public async Task<Branch> GetByNormalizedDigitalMenu(string normalizedDigitalMenu)
        {
            Console.WriteLine($"Iniciando GetByDigitalMenuLink con digitalMenuLink: {normalizedDigitalMenu}");

            // Intentar obtener la sucursal por su enlace de menú digital
            var branch = await context.Branches
                                      .Include(b => b.Brand) // Incluir la marca relacionada si es necesario para tu lógica de negocio
                                      .FirstOrDefaultAsync(b => b.NormalizedDigitalMenu == normalizedDigitalMenu);

            Console.WriteLine(branch != null ? $"Sucursal encontrada: {branch.Name}" : "Sucursal no encontrada con el enlace de menú digital: {digitalMenuLink}");

            return branch ?? throw new Exception("Sucursal no encontrada con el enlace de menú digital proporcionado.");
        }


        public async Task<Brand> GetBrandByBranchId(Guid branchId)
        {
            Console.WriteLine($"Iniciando GetBrandByBranchId con branchId: {branchId}");

            // Verificar si la sucursal existe
            var branchExists = await context.Branches.AnyAsync(b => b.Id == branchId);
            Console.WriteLine(branchExists ? "La sucursal existe." : "La sucursal no existe.");

            if (!branchExists)
            {
                Console.WriteLine($"No se encontró una sucursal con el ID: {branchId}");
                throw new Exception("Sucursal no encontrada.");
            }

            // Obtener la marca asociada a la sucursal
            var brand = await context.Branches
                                     .Where(b => b.Id == branchId)
                                     .Include(b => b.Brand)
                                     .Select(b => b.Brand)
                                     .FirstOrDefaultAsync();

            Console.WriteLine(brand != null ? $"Marca encontrada: {brand.Name}" : "Marca no encontrada para la sucursal con ID: {branchId}");

            return brand ?? throw new Exception("Marca no asociada a la sucursal.");
        }

        public async Task<BranchCatalogResponse?> GetDigitalMenuByBranchId(Guid branchId)
        {
            var branch = await context.Branches.FirstOrDefaultAsync(b => b.Id == branchId);
            if (branch == null || string.IsNullOrWhiteSpace(branch.DigitalMenuJson))
            {
                return null;
            }

            try
            {
                var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);
                return digitalMenu;
            }
            catch (JsonException)
            {
                // Logear o manejar el error de deserialización como consideres apropiado
                return null;
            }
        }


    }
}
