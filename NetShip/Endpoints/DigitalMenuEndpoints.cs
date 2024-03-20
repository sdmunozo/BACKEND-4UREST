using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetShip.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NetShip.Entities;

namespace NetShip.Endpoints
{
    public static class DigitalMenuEndpoints
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public static RouteGroupBuilder MapDigitalMenu(this RouteGroupBuilder group)
        {
            group.MapGet("/digital-menu/{normalizedDigitalMenu}", async (HttpContext httpContext, string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper) =>
                await GetBranchCatalogResponse(httpContext, normalizedDigitalMenu, branchRepository, mapper))
                 .WithName("GetBranchCatalogResponse")
                 .Produces<DTOs.DigitalMenu.BranchCatalogResponse>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status404NotFound);

            return group;
        }


        private static async Task<IResult> GetBranchCatalogResponse(HttpContext httpContext, string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper)
        {
            var branch = await branchRepository.GetByNormalizedDigitalMenu(normalizedDigitalMenu);
            if (branch == null)
            {
                return Results.NotFound("Sucursal no encontrada.");
            }

            var brand = await branchRepository.GetBrandByBranchId(branch.Id);
            if (brand == null)
            {
                return Results.NotFound("Marca no encontrada para la sucursal dada.");
            }

            var catalogs = await branchRepository.GetCatalogsByBranchId(branch.Id);
            var catalogsWithCategoriesTasks = catalogs.Select(catalog => ProcessCatalogAsync(catalog, httpContext.RequestServices, mapper)).ToList();

            var catalogsWithCategories = await Task.WhenAll(catalogsWithCategoriesTasks);

            var response = mapper.Map<DTOs.DigitalMenu.BranchCatalogResponse>(branch);
            response.Catalogs = catalogsWithCategories.ToList();
            return Results.Ok(response);
        }

        private static async Task<DTOs.DigitalMenu.Catalog> ProcessCatalogAsync(Catalog catalog, IServiceProvider serviceProvider, IMapper mapper)
        {
            var categoriesWithItems = new List<DTOs.DigitalMenu.Category>();

            // Crea un nuevo ámbito de servicio por cada categoría para evitar conflictos de concurrencia
            foreach (var categoryEntity in catalog.Categories)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var branchRepository = scope.ServiceProvider.GetRequiredService<IBranchesRepository>();

                    // Asíncronamente obtén los ítems para cada categoría
                    var items = await branchRepository.GetItemsByCategoryId(categoryEntity.Id);

                    // Mapea la categoría y sus ítems a los DTOs correspondientes
                    var categoryDto = mapper.Map<DTOs.DigitalMenu.Category>(categoryEntity);
                    categoryDto.Items = mapper.Map<List<DTOs.DigitalMenu.Item>>(items);

                    categoriesWithItems.Add(categoryDto);
                }
            }

            // Mapea el catálogo y asigna las categorías procesadas
            var catalogDto = mapper.Map<DTOs.DigitalMenu.Catalog>(catalog);
            catalogDto.Categories = categoriesWithItems;
            return catalogDto;
        }



    }
}



/* se obtiene hasta categorias
 
         private static async Task<IResult> GetBranchCatalogResponse(string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper)
        {
            var branch = await branchRepository.GetByNormalizedDigitalMenu(normalizedDigitalMenu);
            if (branch == null)
            {
                return Results.NotFound("Sucursal no encontrada.");
            }

            var brand = await branchRepository.GetBrandByBranchId(branch.Id);
            if (brand == null)
            {
                return Results.NotFound("Marca no encontrada para la sucursal dada.");
            }

            var catalogs = await branchRepository.GetCatalogsByBranchId(branch.Id);
            var catalogsWithCategoriesTasks = catalogs.Select(catalog => ProcessCatalogAsync(catalog, branchRepository, mapper)).ToList();

            var catalogsWithCategories = await Task.WhenAll(catalogsWithCategoriesTasks);

            var response = mapper.Map<BranchCatalogResponse>(branch);
            response.Catalogs = catalogsWithCategories.ToList();
            return Results.Ok(response);
        }

        private static async Task<DTOs.DigitalMenu.Catalog> ProcessCatalogAsync(NetShip.Entities.Catalog catalog, IBranchesRepository branchRepository, IMapper mapper)
        {
            await semaphore.WaitAsync();
            try
            {
                var categories = await branchRepository.GetCategoriesByCatalogId(catalog.Id);
                var catalogDto = mapper.Map<DTOs.DigitalMenu.Catalog>(catalog);
                catalogDto.Categories = mapper.Map<List<DTOs.DigitalMenu.Category>>(categories);
                return catalogDto;
            }
            finally
            {
                semaphore.Release();
            }
        }
 */

/* retorna catalogos
         private static async Task<IResult> GetBranchCatalogResponse(string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper)
        {
            var branch = await branchRepository.GetByNormalizedDigitalMenu(normalizedDigitalMenu);
            if (branch == null)
            {
                return Results.NotFound("Sucursal no encontrada.");
            }

            var brand = await branchRepository.GetBrandByBranchId(branch.Id);
            if (brand == null)
            {
                return Results.NotFound("Marca no encontrada para la sucursal dada.");
            }

            var catalogs = await branchRepository.GetCatalogsByBranchId(branch.Id);
            var catalogsWithCategories = catalogs.Select(catalog =>
            {
                return mapper.Map<DTOs.DigitalMenu.Catalog>(catalog);
            }).ToList();

            var response = mapper.Map<BranchCatalogResponse>(branch);
            response.Catalogs = catalogsWithCategories;
            return Results.Ok(response);
        }
 */


/* retorna info de brand
private static async Task<IResult> GetBranchCatalogResponse(string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper)
{
    var branch = await branchRepository.GetByNormalizedDigitalMenu(normalizedDigitalMenu);
    if (branch == null)
    {
        return Results.NotFound("Sucursal no encontrada.");
    }

    var brand = await branchRepository.GetBrandByBranchId(branch.Id);
    if (brand == null)
    {
        return Results.NotFound("Marca no encontrada para la sucursal dada.");
    }
    var response = mapper.Map<BranchCatalogResponse>(branch);
    return Results.Ok(response);
} */