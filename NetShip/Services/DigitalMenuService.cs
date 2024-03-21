using NetShip.DTOs.DigitalMenu;
using NetShip.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Catalog = NetShip.DTOs.DigitalMenu.Catalog;

namespace NetShip.Services
{
    public class DigitalMenuService
    {
        private readonly ApplicationDbContext _context;

        public DigitalMenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateInitialDigitalMenuJson(Guid branchId)
        {
            var branch = await _context.Branches
                                       .Include(b => b.Brand)
                                       .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null) throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = new BranchCatalogResponse
            {
                BrandName = branch.Brand.Name ?? string.Empty,
                BranchName = branch.Name,
                InstagramLink = branch.Brand.Instagram ?? string.Empty,
                FacebookLink = branch.Brand.Facebook ?? string.Empty,
                WebsiteLink = branch.Brand.Website ?? string.Empty,
                BrandLogo = branch.Brand.Logo ?? string.Empty,
                BrandSlogan = branch.Brand.Slogan ?? string.Empty,
                MenuBackground = branch.Brand.CatalogsBackground ?? string.Empty,
                Catalogs = new System.Collections.Generic.List<Catalog>()
            };

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForCatalog(Guid branchId)
        {
            var branch = await _context.Branches
                                       .Include(b => b.Brand)
                                       .Include(b => b.Catalogs).ThenInclude(c => c.Categories).ThenInclude(cat => cat.Items)
                                       .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null) throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();
            digitalMenu.Catalogs = branch.Catalogs.Select(catalog => new Catalog
            {
                Name = catalog.Name,
                Description = catalog.Description,
                Icon = catalog.Icon,
                Categories = new List<DTOs.DigitalMenu.Category>() 
            }).ToList();

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForCategory(Guid catalogId)
        {
            // Recuperar el catálogo y su sucursal correspondiente, sin incluir las categorías en este punto
            var catalog = await _context.Catalogs
                                        .Include(c => c.Branch)
                                            .ThenInclude(b => b.Brand)
                                        .FirstOrDefaultAsync(c => c.Id == catalogId);

            if (catalog == null || catalog.Branch == null)
            {
                throw new ArgumentException("Catálogo no encontrado o no asociado correctamente con una sucursal.", nameof(catalogId));
            }

            // Recuperar las categorías actuales directamente desde la base de datos para este catálogo
            var currentCategories = await _context.Categories
                                                  .Where(cat => cat.CatalogId == catalogId)
                                                  .ToListAsync();

            var branch = catalog.Branch;
            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            // Encontrar el catálogo específico dentro del JSON y actualizar sus categorías
            var updatedCatalogs = digitalMenu.Catalogs.Select(c =>
            {
                if (c.Name == catalog.Name)
                {
                    // Actualizar la lista de categorías basada en las categorías actuales de la base de datos
                    var updatedCategories = currentCategories.Select(cat => new DTOs.DigitalMenu.Category
                    {
                        Name = cat.Name,
                        Description = cat.Description,
                        Icon = cat.Icon,
                        Items = new List<DTOs.DigitalMenu.Item>(), // Lista de ítems vacía, asumiendo que se actualizará en otro lugar si es necesario
                        Products = new List<DTOs.DigitalMenu.Product>() // Lista de productos vacía por el mismo motivo
                    }).ToList();

                    // Retornar el catálogo actualizado con las categorías actualizadas
                    return new Catalog
                    {
                        Name = catalog.Name,
                        Description = catalog.Description,
                        Icon = catalog.Icon,
                        Categories = updatedCategories
                    };
                }
                return c;
            }).ToList();

            // Actualizar el JSON del menú digital con los catálogos actualizados
            digitalMenu.Catalogs = updatedCatalogs;
            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }




        public async Task UpdateDigitalMenuJsonForBrand(Guid brandId)
        {
            var branches = await _context.Branches
                                         .Where(b => b.BrandId == brandId)
                                         .Include(b => b.Brand)
                                         .ToListAsync();

            foreach (var branch in branches)
            {
                var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

                digitalMenu.BrandName = branch.Brand.Name ?? string.Empty;
                digitalMenu.InstagramLink = branch.Brand.Instagram ?? string.Empty;
                digitalMenu.FacebookLink = branch.Brand.Facebook ?? string.Empty;
                digitalMenu.WebsiteLink = branch.Brand.Website ?? string.Empty;
                digitalMenu.BrandLogo = branch.Brand.Logo ?? string.Empty;
                digitalMenu.BrandSlogan = branch.Brand.Slogan ?? string.Empty;
                digitalMenu.MenuBackground = branch.Brand.CatalogsBackground ?? string.Empty;

                branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            }

            await _context.SaveChangesAsync();
        }
    }
}
