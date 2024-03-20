using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
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

        public async Task<List<Catalog>> GetCatalogsByBranchId(Guid branchId)
        {
            Console.WriteLine($"Iniciando GetCatalogsByBranchId con branchId: {branchId}");

            // Verificar si la sucursal existe
            var branchExists = await context.Branches.AnyAsync(b => b.Id == branchId);
            Console.WriteLine(branchExists ? "La sucursal existe." : "La sucursal no existe.");

            if (!branchExists)
            {
                Console.WriteLine($"No se encontró una sucursal con el ID: {branchId}");
                throw new Exception("Sucursal no encontrada.");
            }

            // Obtener los catálogos asociados a la sucursal
            var catalogs = await context.Branches
                                        .Where(b => b.Id == branchId)
                                        .Include(b => b.Catalogs) // Asumiendo que hay una propiedad Catalogs en Branch
                                        .SelectMany(b => b.Catalogs) // Utiliza SelectMany ya que esperamos obtener una lista de catálogos
                                        .ToListAsync();

            if (catalogs == null || catalogs.Count == 0)
            {
                Console.WriteLine($"Catálogos no encontrados para la sucursal con ID: {branchId}");
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new List<Catalog>();
            }

            Console.WriteLine($"Se encontraron {catalogs.Count} catálogos para la sucursal con ID: {branchId}");
            return catalogs;
        }

        public async Task<List<Category>> GetCategoriesByCatalogId(Guid catalogId)
        {
            Console.WriteLine($"Iniciando GetCategoriesByCatalogId con catalogId: {catalogId}");

            // Verificar si el catálogo existe
            var catalogExists = await context.Catalogs.AnyAsync(c => c.Id == catalogId);
            Console.WriteLine(catalogExists ? "El catálogo existe." : "El catálogo no existe.");

            if (!catalogExists)
            {
                Console.WriteLine($"No se encontró un catálogo con el ID: {catalogId}");
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new List<Category>();
            }

            // Obtener las categorías asociadas al catálogo
            var categories = await context.Catalogs
                                          .Where(c => c.Id == catalogId)
                                          .Include(c => c.Categories) // Asumiendo que hay una propiedad Categories en Catalog
                                          .SelectMany(c => c.Categories) // Utiliza SelectMany ya que esperamos obtener una lista de categorías
                                          .ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine($"Categorías no encontradas para el catálogo con ID: {catalogId}");
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new List<Category>();
            }

            Console.WriteLine($"Se encontraron {categories.Count} categorías para el catálogo con ID: {catalogId}");
            return categories;
        }

        public async Task<List<Item>> GetItemsByCategoryId(Guid categoryId)
        {
            Console.WriteLine($"Iniciando GetItemsByCategoryId con categoryId: {categoryId}");

            // Verificar si la categoría existe
            var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId);
            Console.WriteLine(categoryExists ? "La categoría existe." : "La categoría no existe.");

            if (!categoryExists)
            {
                Console.WriteLine($"No se encontró una categoría con el ID: {categoryId}");
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new List<Item>();
            }

            // Obtener los ítems asociados a la categoría
            var items = await context.Categories
                                     .Where(c => c.Id == categoryId)
                                     .Include(c => c.Items) // Asumiendo que hay una propiedad Items en Category
                                     .SelectMany(c => c.Items) // Utiliza SelectMany ya que esperamos obtener una lista de ítems
                                     .ToListAsync();

            if (items == null || items.Count == 0)
            {
                Console.WriteLine($"Ítems no encontrados para la categoría con ID: {categoryId}");
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new List<Item>();
            }

            Console.WriteLine($"Se encontraron {items.Count} ítems para la categoría con ID: {categoryId}");
            return items;
        }





    }
}
