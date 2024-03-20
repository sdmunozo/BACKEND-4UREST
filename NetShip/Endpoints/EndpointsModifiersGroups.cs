using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.ModifiersGroup;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsModifiersGroups
    {
        private static readonly string container = "modifiersGroups";

        public static RouteGroupBuilder MapModifiersGroups(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createModifiersGroup).WithName("CreateModifiersGroup").DisableAntiforgery()
                .RequireAuthorization();

            group.MapPut("/update/{modifiersGroupId:Guid}", updateModifiersGroup).WithName("UpdateModifiersGroup").DisableAntiforgery()
                .RequireAuthorization();

            group.MapGet("/getAll", getAllModifiersGroups).WithName("GetAllModifiersGroups")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-modifiersGroups"))
                .RequireAuthorization();

            group.MapGet("/getById/{modifiersGroupId:Guid}", getModifiersGroupById).WithName("GetModifiersGroupById")
                .RequireAuthorization();

            group.MapDelete("/delete/{modifiersGroupId:Guid}", deleteModifiersGroup).WithName("DeleteModifiersGroup")
                .RequireAuthorization();

            group.MapGet("getByName/{modifiersGroupName}", getModifiersGroupByName).WithName("GetModifiersGroupByName")
                .RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> createModifiersGroup(
                     [FromForm] CrModifiersGroupReqDTO createModifiersGroupDTO,
                     IModifiersGroupsRepository repository,
                     IOutputCacheStore outputCacheStore,
                     IMapper mapper,
                     IFileStorage fileStorage)
        {
            var modifiersGroup = mapper.Map<ModifiersGroup>(createModifiersGroupDTO);

            if (createModifiersGroupDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createModifiersGroupDTO.Icon);
                modifiersGroup.Icon = url;
            }

            await repository.Create(modifiersGroup);
            await outputCacheStore.EvictByTagAsync("get-modifiersGroups", default);

            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> updateModifiersGroup(
            Guid modifiersGroupId,
            [FromForm] UpModifiersGroupReqDTO updateModifiersGroupDTO,
            IModifiersGroupsRepository repository,
            IMapper mapper,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {
            var modifiersGroupToUpdate = await repository.GetById(modifiersGroupId);

            if (modifiersGroupToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateModifiersGroupDTO, modifiersGroupToUpdate);

            if (updateModifiersGroupDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(modifiersGroupToUpdate.Icon, container, updateModifiersGroupDTO.Icon);
                modifiersGroupToUpdate.Icon = iconUrl;
            }

            await repository.Update(modifiersGroupToUpdate);
            await outputCacheStore.EvictByTagAsync("get-modifiersGroups", default);

            return TypedResults.NoContent();
        }



        static async Task<Results<Ok<ModifiersGroupDTO>, NotFound>> getModifiersGroupById(IModifiersGroupsRepository repository, Guid modifiersGroupId, IMapper mapper)
        {
            var modifiersGroup = await repository.GetById(modifiersGroupId);

            if (modifiersGroup == null)
                return TypedResults.NotFound();

            var modifiersGroupDTO = mapper.Map<ModifiersGroupDTO>(modifiersGroup);

            return TypedResults.Ok(modifiersGroupDTO);
        }

        static async Task<Ok<List<ModifiersGroupDTO>>> getAllModifiersGroups(IModifiersGroupsRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var modifiersGroups = await repository.GetAll(pagination);
            var modifiersGroupDTO = mapper.Map<List<ModifiersGroupDTO>>(modifiersGroups);
            return TypedResults.Ok(modifiersGroupDTO);

        }

        static async Task<Ok<List<ModifiersGroupDTO>>> getModifiersGroupByName(string modifiersGroupName, IModifiersGroupsRepository repository, IMapper mapper)
        {
            var modifiersGroups = await repository.GetByName(modifiersGroupName);

            var modifiersGroupDTO = mapper.Map<List<ModifiersGroupDTO>>(modifiersGroups);

            return TypedResults.Ok(modifiersGroupDTO);

        }

        static async Task<Results<NoContent, NotFound>> deleteModifiersGroup(
            Guid modifiersGroupId,
            IModifiersGroupsRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {

            var regDB = await repository.GetById(modifiersGroupId);

            if (regDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(modifiersGroupId);
            await fileStorage.Delete(regDB.Icon, container);
            await outputCacheStore.EvictByTagAsync("get-modifiersGroups", default);
            return TypedResults.NoContent();
        }
    }
}

