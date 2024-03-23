using NetShip.DTOs.DigitalMenu;
using NetShip.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Catalog = NetShip.DTOs.DigitalMenu.Catalog;
using System.Transactions;

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
                BrandId = branch.BrandId.ToString() ?? string.Empty,
                BranchId = branch.Id.ToString() ?? string.Empty,
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

            foreach (var catalog in branch.Catalogs)
            {
                var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == catalog.Id.ToString());
                if (catalogInJson != null)
                {
                    catalogInJson.Name = catalog.Name;
                    catalogInJson.Description = catalog.Description;
                    catalogInJson.Icon = catalog.Icon;
                }
                else
                {
                    digitalMenu.Catalogs.Add(new Catalog
                    {
                        Id = catalog.Id.ToString(),
                        Name = catalog.Name,
                        Description = catalog.Description,
                        Icon = catalog.Icon,
                        Categories = new List<DTOs.DigitalMenu.Category>()
                    });
                }
            }

            digitalMenu.Catalogs = digitalMenu.Catalogs.Where(c => branch.Catalogs.Any(bc => bc.Id.ToString() == c.Id)).ToList();

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForCategory(Guid catalogId)
        {
            var catalog = await _context.Catalogs
                                        .Include(c => c.Categories).ThenInclude(cat => cat.Items)
                                        .Include(c => c.Categories).ThenInclude(cat => cat.Products)
                                        .FirstOrDefaultAsync(c => c.Id == catalogId);

            if (catalog == null) throw new ArgumentException("Catálogo no encontrado.", nameof(catalogId));

            var branch = await _context.Branches
                                       .Include(b => b.Catalogs).ThenInclude(c => c.Categories)
                                       .FirstOrDefaultAsync(b => b.Catalogs.Any(c => c.Id == catalogId));

            if (branch == null) throw new ArgumentException("Sucursal con el catálogo especificado no encontrada.", nameof(catalogId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == catalog.Id.ToString());
            if (catalogInJson != null)
            {
                // Actualizar las categorías existentes
                foreach (var category in catalog.Categories)
                {
                    var categoryInJson = catalogInJson.Categories.FirstOrDefault(cat => cat.Id == category.Id.ToString());
                    if (categoryInJson != null)
                    {
                        // Actualizar los datos de la categoría preservando ítems y productos existentes
                        categoryInJson.Name = category.Name;
                        categoryInJson.Description = category.Description;
                        categoryInJson.Icon = category.Icon;
                    }
                    else
                    {
                        // Si la categoría no existe en el JSON, agregarla como nueva
                        catalogInJson.Categories.Add(new DTOs.DigitalMenu.Category
                        {
                            Id = category.Id.ToString(),
                            Name = category.Name,
                            Description = category.Description,
                            Icon = category.Icon,
                            Items = new List<DTOs.DigitalMenu.Item>(),
                            Products = new List<DTOs.DigitalMenu.Product>()
                            /*
                            Items = category.Items.Select(item => new DTOs.DigitalMenu.Item
                            {
                                Id = item.Id.ToString(),
                                Alias = item.Alias,
                                Description = item.Description,
                                Icon = item.Icon,
                                Price = 0//item.Price
                            }).ToList(),
                            Products = category.Products.Select(product => new DTOs.DigitalMenu.Product
                            {
                                Id = product.Id.ToString(),
                                Alias = product.Alias,
                                Description = product.Description,
                                Icon = product.Icon,
                                // ModifiersGroups pueden ser agregados aquí si es necesario
                            }).ToList()

                            */
                        });
                    }
                }
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCategoryFromDigitalMenuJson(Guid categoryId, Guid branchId)
        {
            var branch = await _context.Branches
                                       .Where(b => b.Id == branchId)
                                       .FirstOrDefaultAsync();

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);

            foreach (var catalog in digitalMenu.Catalogs)
            {
                var categoryToRemove = catalog.Categories.FirstOrDefault(cat => cat.Id == categoryId.ToString());
                if (categoryToRemove != null)
                {
                    catalog.Categories.Remove(categoryToRemove);
                    break;
                }
            }

                branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
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

                digitalMenu.BrandId = branch.BrandId.ToString() ?? string.Empty;
                digitalMenu.BranchId = branch.Id.ToString() ?? string.Empty;
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

        public async Task UpdateDigitalMenuJsonForProduct(Guid productId)
        {
            var product = await _context.Products
                                         .Include(p => p.Category)
                                             .ThenInclude(c => c.Catalog)
                                         .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new ArgumentException("Producto no encontrado.", nameof(productId));

            var branch = await _context.Branches
                                       .Include(b => b.Catalogs)
                                           .ThenInclude(c => c.Categories)
                                               .ThenInclude(cat => cat.Products)
                                       .FirstOrDefaultAsync(b => b.Catalogs.Any(c => c.Id == product.Category.CatalogId));

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada para el producto especificado.", nameof(productId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == product.Category.Catalog.Id.ToString());
            if (catalogInJson == null)
            {
                throw new ArgumentException("Catálogo no encontrado en el JSON del menú digital.", nameof(product.Category.CatalogId));
            }

            var categoryInJson = catalogInJson.Categories.FirstOrDefault(cat => cat.Id == product.Category.Id.ToString());
            if (categoryInJson == null)
            {
                throw new ArgumentException("Categoría no encontrada en el JSON del menú digital.", nameof(product.CategoryId));
            }

            var productInJson = categoryInJson.Products.FirstOrDefault(p => p.Id == productId.ToString());
            if (productInJson != null)
            {
                productInJson.Alias = product.Alias;
                productInJson.Description = product.Description;
                productInJson.Icon = product.Icon;
            }
            else
            {
                categoryInJson.Products.Add(new DTOs.DigitalMenu.Product
                {
                    Id = productId.ToString(),
                    Alias = product.Alias ?? string.Empty,
                    Description = product.Description ?? string.Empty,
                    Icon = product.Icon ?? string.Empty,
                    ModifiersGroups = new List<DTOs.DigitalMenu.ModifiersGroup>()
                });
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductFromDigitalMenuJson(Guid productId, Guid branchId)
        {
            var branch = await _context.Branches
                                       .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);
            if (digitalMenu == null)
                throw new ArgumentException("Error al deserializar el menú digital.", nameof(branch.DigitalMenuJson));

            foreach (var catalog in digitalMenu.Catalogs)
            {
                foreach (var category in catalog.Categories)
                {
                    var productToRemove = category.Products.FirstOrDefault(p => p.Id == productId.ToString());
                    if (productToRemove != null)
                    {
                        category.Products.Remove(productToRemove);
                        break; // Asumiendo que un producto solo pertenece a una categoría
                    }
                }
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForModifiersGroup(Guid modifiersGroupId)
        {
            var modifiersGroup = await _context.ModifiersGroups
                .Include(mg => mg.Product)
                    .ThenInclude(p => p.Category)
                        .ThenInclude(c => c.Catalog)
                            .ThenInclude(cat => cat.Branch)
                .FirstOrDefaultAsync(mg => mg.Id == modifiersGroupId);

            if (modifiersGroup == null || modifiersGroup.Product?.Category?.Catalog?.Branch == null)
                throw new ArgumentException("Grupo de modificadores no encontrado o sin producto/categoría/catálogo/sucursal asociado.", nameof(modifiersGroupId));

            var branch = modifiersGroup.Product.Category.Catalog.Branch;
            var branchId = branch.Id;

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == modifiersGroup.Product.Category.Catalog.Id.ToString());
            if (catalogInJson == null) throw new ArgumentException("Catálogo no encontrado en el JSON del menú digital.", nameof(modifiersGroup.Product.Category.CatalogId));

            var categoryInJson = catalogInJson.Categories.FirstOrDefault(cat => cat.Id == modifiersGroup.Product.Category.Id.ToString());
            if (categoryInJson == null) throw new ArgumentException("Categoría no encontrada en el JSON del menú digital.", nameof(modifiersGroup.Product.CategoryId));

            var productInJson = categoryInJson.Products.FirstOrDefault(p => p.Id == modifiersGroup.Product.Id.ToString());
            if (productInJson == null) throw new ArgumentException("Producto no encontrado en el JSON del menú digital.", nameof(modifiersGroup.ProductId));

            var modifiersGroupInJson = productInJson.ModifiersGroups.FirstOrDefault(mg => mg.Id == modifiersGroupId.ToString());
            if (modifiersGroupInJson != null)
            {
                modifiersGroupInJson.Alias = modifiersGroup.Alias ?? "";
                modifiersGroupInJson.Description = modifiersGroup.Description ?? "";
                modifiersGroupInJson.Icon = modifiersGroup.Icon ?? "";
                
                /*modifiersGroupInJson.Modifiers = modifiersGroup.Modifiers.Select(modifier => new DTOs.DigitalMenu.Modifier
                {
                    Id = modifier.Id.ToString(),
                    Alias = modifier.Alias ?? "",
                    Description = modifier.Description ?? "",
                    Icon = modifier.Icon ?? "",
                    Price = "-"
                }).ToList();

                */
            }
            else
            {
                productInJson.ModifiersGroups.Add(new DTOs.DigitalMenu.ModifiersGroup
                {
                    Id = modifiersGroup.Id.ToString(),
                    Alias = modifiersGroup.Alias ?? "",
                    Description = modifiersGroup.Description ?? "",
                    Icon = modifiersGroup.Icon ?? "",
                    Modifiers = new List<DTOs.DigitalMenu.Modifier>()
                });
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveModifiersGroupFromDigitalMenuJson(Guid modifiersGroupId, Guid branchId)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);
            if (digitalMenu == null)
                throw new InvalidOperationException("Error al deserializar el menú digital.");

            foreach (var catalog in digitalMenu.Catalogs)
            {
                foreach (var category in catalog.Categories)
                {
                    foreach (var product in category.Products)
                    {
                        var modifiersGroupToRemove = product.ModifiersGroups.FirstOrDefault(mg => mg.Id == modifiersGroupId.ToString());
                        if (modifiersGroupToRemove != null)
                        {
                            product.ModifiersGroups.Remove(modifiersGroupToRemove);
                            break;
                        }
                    }
                }
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForModifier(Guid modifierId)
        {
            var modifier = await _context.Modifiers
                                         .Include(mg => mg.ModifiersGroup)
                                            .ThenInclude(p => p.Product)
                                            .ThenInclude(c => c.Category)
                                             .ThenInclude(c => c.Catalog)
                                         .FirstOrDefaultAsync(m => m.Id == modifierId);

            if (modifier == null)
                throw new ArgumentException("Modificador no encontrado.", nameof(modifierId));

            var branch = await _context.Branches
                                       .Include(b => b.Catalogs)
                                           .ThenInclude(c => c.Categories)
                                               .ThenInclude(cat => cat.Products)
                                                   .ThenInclude(cat => cat.ModifiersGroups)
                                       .FirstOrDefaultAsync(b => b.Catalogs.Any(c => c.Id == modifier.ModifiersGroup.Product.Category.CatalogId));

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada para el modificador especificado.", nameof(modifierId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == modifier.ModifiersGroup.Product.Category.Catalog.Id.ToString());
            if (catalogInJson == null)
            {
                throw new ArgumentException("Catálogo no encontrado en el JSON del menú digital.", nameof(modifier.ModifiersGroup.Product.Category.CatalogId));
            }

            var categoryInJson = catalogInJson.Categories.FirstOrDefault(cat => cat.Id == modifier.ModifiersGroup.Product.Category.Id.ToString());
            if (categoryInJson == null)
            {
                throw new ArgumentException("Categoría no encontrada en el JSON del menú digital.", nameof(modifier.ModifiersGroup.Product.CategoryId));
            }

            var productInJson = categoryInJson.Products.FirstOrDefault(cat => cat.Id == modifier.ModifiersGroup.Product.Id.ToString());
            if (categoryInJson == null)
            {
                throw new ArgumentException("Producto no encontrado en el JSON del menú digital.", nameof(modifier.ModifiersGroup.ProductId));
            }

            var groupModifierInJson = productInJson!.ModifiersGroups.FirstOrDefault(cat => cat.Id == modifier.ModifiersGroup.Id.ToString());
            if (categoryInJson == null)
            {
                throw new ArgumentException("Grupo de Modificadores no encontrado en el JSON del menú digital.", nameof(modifier.ModifiersGroupId));
            }

            var modifierInJson = groupModifierInJson!.Modifiers.FirstOrDefault(p => p.Id == modifierId.ToString());

            /*
            var basePlatform = await _context.Platforms
                        .FirstOrDefaultAsync(p => p.BrandId == branch.BrandId && p.Alias == "Base");

            if (basePlatform == null)
            {
                throw new ArgumentException("Plataforma base no encontrada para la marca.", nameof(branch.BrandId));
            }

            string modifierPriceOnBasePlatform = (modifier.PricePerModifierPerPlatforms
                .FirstOrDefault(ppmpp => ppmpp.PlatformId == basePlatform.Id && ppmpp.IsActive)?.Price ?? 0).ToString();

            */
            if (modifierInJson != null)
            {
                modifierInJson.Alias = modifier.Alias ?? string.Empty;
                modifierInJson.Description = modifier.Description ?? string.Empty;
                modifierInJson.Icon = modifier.Icon ?? string.Empty;
                modifierInJson.Price = modifier.Price.ToString() ?? string.Empty;
            }
            else
            {
                groupModifierInJson.Modifiers.Add(new DTOs.DigitalMenu.Modifier
                {
                    Id = modifier.Id.ToString() ?? string.Empty,
                    Alias = modifier.Alias ?? string.Empty,
                    Description = modifier.Description ?? string.Empty,
                    Icon = modifier.Icon ?? string.Empty,
                    Price = modifier.Price.ToString() ?? string.Empty
            });
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveModifierFromDigitalMenuJson(Guid modifierId, Guid branchId)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);

            if (digitalMenu == null)
                throw new InvalidOperationException("Error al deserializar el menú digital.");

            foreach (var catalog in digitalMenu.Catalogs)
            {
                foreach (var category in catalog.Categories)
                {
                    foreach (var product in category.Products)
                    {
                        foreach (var modifiersGroup in product.ModifiersGroups)
                        {
                            var modifierToRemove = modifiersGroup.Modifiers.FirstOrDefault(mg => mg.Id == modifierId.ToString());
                            
                            if (modifierToRemove != null)
                            {
                                modifiersGroup.Modifiers.Remove(modifierToRemove);
                                break;
                            }
                        }
                    }
                }
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDigitalMenuJsonForItem(Guid itemId)
        {
            var item = await _context.Items
                                         .Include(mg => mg.Category)
                                            .ThenInclude(p => p.Catalog)
                                         .FirstOrDefaultAsync(m => m.Id == itemId);

            if (item == null)
                throw new ArgumentException("Articulo no encontrado.", nameof(itemId));

            var branch = await _context.Branches
                                       .Include(b => b.Catalogs)
                                           .ThenInclude(c => c.Categories)
                                       .FirstOrDefaultAsync(b => b.Catalogs.Any(c => c.Id == item.Category.CatalogId));

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada para el Articulo especificado.", nameof(itemId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson) ?? new BranchCatalogResponse();

            var catalogInJson = digitalMenu.Catalogs.FirstOrDefault(c => c.Id == item.Category.Catalog.Id.ToString());
            if (catalogInJson == null)
            {
                throw new ArgumentException("Catálogo no encontrado en el JSON del menú digital.", nameof(item.Category.CatalogId));
            }

            var categoryInJson = catalogInJson.Categories.FirstOrDefault(cat => cat.Id == item.Category.Id.ToString());
            if (categoryInJson == null)
            {
                throw new ArgumentException("Categoría no encontrada en el JSON del menú digital.", nameof(item.CategoryId));
            }

            var itemInJson = categoryInJson!.Items.FirstOrDefault(p => p.Id == itemId.ToString());

            /*
            var basePlatform = await _context.Platforms
                       .FirstOrDefaultAsync(p => p.BrandId == branch.BrandId && p.Alias == "Base");

            if (basePlatform == null)
            {
                throw new ArgumentException("Plataforma base no encontrada para la marca.", nameof(branch.BrandId));
            }

            string itemPriceOnBasePlatform = (item.PricePerItemPerPlatforms
                .FirstOrDefault(ppmpp => ppmpp.PlatformId == basePlatform.Id && ppmpp.IsActive)?.Price ?? 0).ToString();
            */
            if (itemInJson != null)
            {
                itemInJson.Alias = item.Alias ?? string.Empty;
                itemInJson.Description = item.Description ?? string.Empty;
                itemInJson.Icon = item.Icon ?? string.Empty;
                itemInJson.Price = item.Price.ToString() ?? string.Empty;
            }
            else
            {
                categoryInJson.Items.Add(new DTOs.DigitalMenu.Item
                {
                    Id = item.Id.ToString() ?? string.Empty,
                    Alias = item.Alias ?? string.Empty,
                    Description = item.Description ?? string.Empty,
                    Icon = item.Icon ?? string.Empty,
                    Price = item.Price.ToString() ?? string.Empty
            });
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromDigitalMenuJson(Guid itemId, Guid branchId)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
                throw new ArgumentException("Sucursal no encontrada.", nameof(branchId));

            var digitalMenu = JsonConvert.DeserializeObject<BranchCatalogResponse>(branch.DigitalMenuJson);

            if (digitalMenu == null)
                throw new InvalidOperationException("Error al deserializar el menú digital.");

            foreach (var catalog in digitalMenu.Catalogs)
            {
                foreach (var category in catalog.Categories)
                {
                    var itemToRemove = category.Items.FirstOrDefault(mg => mg.Id == itemId.ToString());

                    if (itemToRemove != null)
                    {
                        category.Items.Remove(itemToRemove);
                        break;
                    }
                }
            }

            branch.DigitalMenuJson = JsonConvert.SerializeObject(digitalMenu);
            await _context.SaveChangesAsync();
        }

    }
}