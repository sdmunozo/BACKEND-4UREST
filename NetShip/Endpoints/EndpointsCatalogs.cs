using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetShip.DTOs.CatalogDTOs;
using NetShip.Entities;
using NetShip.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace NetShip.Endpoints
{
    public static class EndpointsCatalogs
    {
        private static readonly string container = "catalogs";

        public static RouteGroupBuilder MapCatalogs(this RouteGroupBuilder group)
        {
            group.MapGet("/catalogs/{branchId:guid}", getCatalogsByBranch)
                 .WithName("GetCatalogsByBranch")
                 .Produces<ListOfCatalogsDTO>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status404NotFound).RequireAuthorization();

            group.MapPost("/catalogs", createCatalog)
                 .DisableAntiforgery()
                 .WithName("CreateCatalog");

            group.MapPut("/catalogs/{id:guid}", updateCatalog)
                 .DisableAntiforgery()
                 .WithName("UpdateCatalog");

            group.MapDelete("/catalogs/{id:guid}", deleteCatalog)
                 .WithName("DeleteCatalog");

            return group;
        }

        static async Task<IResult> getCatalogsByBranch(Guid branchId, ICatalogsRepository catalogsRepository, IMapper mapper)
        {
            var catalogsDTO = await catalogsRepository.GetCatalogsByBranchId(branchId);
            if (catalogsDTO.Catalogs == null || catalogsDTO.Catalogs.Count == 0)
            {
                return Results.NotFound("No catalogs found for the given branch.");
            }

            return Results.Ok(catalogsDTO);
        }

        static async Task<IResult> createCatalog([FromBody] CreateCatalogDTO createCatalogDTO, ICatalogsRepository catalogsRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            var catalog = mapper.Map<Catalog>(createCatalogDTO);
            await catalogsRepository.CreateCatalog(catalog);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            var catalogDTO = mapper.Map<CatalogDetailsDTO>(catalog);
            return Results.Created($"/catalogs/{catalog.Id}", catalogDTO);
        }

        static async Task<IResult> updateCatalog(Guid id, [FromBody] UpdateCatalogDTO updateCatalogDTO, ICatalogsRepository catalogsRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            var existingCatalog = await catalogsRepository.GetById(id);
            if (existingCatalog == null)
            {
                return Results.NotFound("Catalog not found.");
            }

            mapper.Map(updateCatalogDTO, existingCatalog);
            await catalogsRepository.UpdateCatalog(existingCatalog);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            return Results.NoContent();
        }

        static async Task<IResult> deleteCatalog(Guid id, ICatalogsRepository catalogsRepository, IOutputCacheStore outputCacheStore)
        {
            var catalog = await catalogsRepository.GetById(id);
            if (catalog == null)
            {
                return Results.NotFound("Catalog not found.");
            }

            await catalogsRepository.DeleteCatalog(id);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            return Results.NoContent();
        }
    }
}
