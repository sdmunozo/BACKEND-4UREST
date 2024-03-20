using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Item;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;
using NetShip.DTOs.Items;
using System;

namespace NetShip.Endpoints
{
    public static class EndpointsItems
    {
        private static readonly string container = "items";

        public static RouteGroupBuilder MapItems(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createItem).WithName("CreateItem").DisableAntiforgery()
                .RequireAuthorization();

            group.MapPut("/update/{itemId:Guid}", updateItem).WithName("UpdateItem").DisableAntiforgery()
                .RequireAuthorization();

            group.MapGet("/getAll", getAllItems).WithName("GetAllItems")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-items"))
                .RequireAuthorization();

            group.MapGet("/getById/{itemId:Guid}", getItemById).WithName("GetItemById")
                .RequireAuthorization();

            group.MapDelete("/delete/{itemId:Guid}", deleteItem).WithName("DeleteItem")
                .RequireAuthorization();

            group.MapGet("getByName/{itemName}", getItemByName).WithName("GetItemByName")
                .RequireAuthorization();

            group.MapPost("setPlatform/{itemId:Guid}", SetPlatform).WithName("SetPlatformItem")
                .RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> createItem(
                     [FromForm] CrItemReqDTO createItemDTO,
                     IItemsRepository repository,
                     IOutputCacheStore outputCacheStore,
                     IMapper mapper,
                     IFileStorage fileStorage)
        {
            var item = mapper.Map<Item>(createItemDTO);

            if (createItemDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createItemDTO.Icon);
                item.Icon = url;
            }

            await repository.Create(item);
            await outputCacheStore.EvictByTagAsync("get-items", default);

            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> updateItem(
            Guid itemId,
            [FromForm] UpItemReqDTO updateItemDTO,
            IItemsRepository repository,
            IMapper mapper,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {
            var itemToUpdate = await repository.GetById(itemId);

            if (itemToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateItemDTO, itemToUpdate);

            if (updateItemDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(itemToUpdate.Icon, container, updateItemDTO.Icon);
                itemToUpdate.Icon = iconUrl;
            }

            await repository.Update(itemToUpdate);
            await outputCacheStore.EvictByTagAsync("get-items", default);

            return TypedResults.NoContent();
        }



        static async Task<Results<Ok<ItemDTO>, NotFound>> getItemById(IItemsRepository repository, Guid itemId, IMapper mapper)
        {
            var item = await repository.GetById(itemId);

            if (item == null)
                return TypedResults.NotFound();

            var itemDTO = mapper.Map<ItemDTO>(item);

            return TypedResults.Ok(itemDTO);
        }

        static async Task<Ok<List<ItemDTO>>> getAllItems(IItemsRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var items = await repository.GetAll(pagination);
            var itemDTO = mapper.Map<List<ItemDTO>>(items);
            return TypedResults.Ok(itemDTO);

        }

        static async Task<Ok<List<ItemDTO>>> getItemByName(string itemName, IItemsRepository repository, IMapper mapper)
        {
            var items = await repository.GetByName(itemName);

            var itemDTO = mapper.Map<List<ItemDTO>>(items);

            return TypedResults.Ok(itemDTO);

        }

        static async Task<Results<NoContent, NotFound>> deleteItem(
            Guid itemId,
            IItemsRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {

            var regDB = await repository.GetById(itemId);

            if (regDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(itemId);
            await fileStorage.Delete(regDB.Icon, container);
            await outputCacheStore.EvictByTagAsync("get-items", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> SetPlatform(Guid itemId, List<SetPlatformItem> setPlatforms,
            IItemsRepository itemsRepository, IPlatformsRepository platformsRepository, IMapper mapper)
        {
            if (!await itemsRepository.Exist(itemId))
            {
                return TypedResults.NotFound();
            }

            var existingPlatforms = new List<Guid>();
            var platformsIds = setPlatforms.Select(x => x.PlatformId).ToList();

            if (setPlatforms.Count != 0)
            {
                existingPlatforms = await platformsRepository.CheckAll(platformsIds);
            }

            if (existingPlatforms.Count != setPlatforms.Count)
            {
                var noExistingPlatforms = platformsIds.Except(existingPlatforms);

                return TypedResults.BadRequest($"Las plataformas de id {string.Join(",", noExistingPlatforms)} no existen.");
            }

            var platforms = mapper.Map<List<PricePerItemPerPlatform>>(setPlatforms);

            await itemsRepository.SetPlatforms(itemId, platforms);
            return TypedResults.NoContent();
        }
    }
}

