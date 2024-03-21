using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetShip.DTOs.CatalogDTOs;
using NetShip.Entities;
using NetShip.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsCatalogs
    {
        private static readonly string container = "catalogs";

        public static RouteGroupBuilder MapCatalogs(this RouteGroupBuilder group)
        {
            group.MapGet("/getAll/{branchId:guid}", getCatalogsByBranch)
                 .WithName("GetCatalogsByBranch")
                 .Produces<ListOfCatalogsDTO>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status404NotFound).RequireAuthorization();

            group.MapPost("/create", createCatalog)
                 .DisableAntiforgery()
                 .WithName("CreateCatalog");

            group.MapPut("/update/{id:guid}", updateCatalog)
                 .DisableAntiforgery()
                 .WithName("UpdateCatalog");

            group.MapDelete("/detele/{id:guid}", deleteCatalog)
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

        static async Task<IResult> createCatalog(DigitalMenuService digitalMenuService, [FromForm] CreateCatalogDTO createCatalogDTO, ICatalogsRepository catalogsRepository, IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
        {
            var catalog = mapper.Map<Catalog>(createCatalogDTO);

            if (createCatalogDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Upload(container, createCatalogDTO.Icon);
                catalog.Icon = iconUrl;
            }

            await catalogsRepository.CreateCatalog(catalog);
            await digitalMenuService.UpdateDigitalMenuJsonForCatalog(catalog.BranchId);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            var catalogDTO = mapper.Map<CatalogDetailsDTO>(catalog);
            return Results.Created($"/catalogs/{catalog.Id}", catalogDTO);
        }


        static async Task<IResult> updateCatalog(DigitalMenuService digitalMenuService, Guid id, [FromForm] UpdateCatalogDTO updateCatalogDTO, ICatalogsRepository catalogsRepository, IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
        {
            var existingCatalog = await catalogsRepository.GetById(id);
            if (existingCatalog == null)
            {
                return Results.NotFound("Catalog not found.");
            }

            mapper.Map(updateCatalogDTO, existingCatalog);

            if (updateCatalogDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(existingCatalog.Icon, container, updateCatalogDTO.Icon);
                existingCatalog.Icon = iconUrl;
            }

            await catalogsRepository.UpdateCatalog(existingCatalog);
            await digitalMenuService.UpdateDigitalMenuJsonForCatalog(existingCatalog.BranchId);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            return Results.NoContent();
        }


        static async Task<IResult> deleteCatalog(DigitalMenuService digitalMenuService, Guid id, ICatalogsRepository catalogsRepository, IOutputCacheStore outputCacheStore)
        {
            Console.WriteLine(id);

            var catalog = await catalogsRepository.GetById(id);
            if (catalog == null)
            {
                return Results.NotFound("Catalog not found.");
            }

            await catalogsRepository.DeleteCatalog(id);
            await digitalMenuService.UpdateDigitalMenuJsonForCatalog(catalog.BranchId);
            await outputCacheStore.EvictByTagAsync("get-catalogs", default);

            return Results.NoContent();
        }
    }
}
