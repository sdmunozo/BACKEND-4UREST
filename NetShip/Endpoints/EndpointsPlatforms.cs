using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Platform;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsPlatforms
    {
        private static readonly string container = "platforms";

        public static RouteGroupBuilder MapPlatforms(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createPlatform).WithName("CreatePlatform").DisableAntiforgery()
                .RequireAuthorization();

            group.MapPut("/update/{platformId:Guid}", updatePlatform).WithName("UpdatePlatform").DisableAntiforgery()
                .RequireAuthorization();

            group.MapGet("/getAll", getAllPlatforms).WithName("GetAllPlatforms")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-platforms"))
                .RequireAuthorization();

            group.MapGet("/getById/{platformId:Guid}", getPlatformById).WithName("GetPlatformById")
                .RequireAuthorization();

            group.MapDelete("/delete/{platformId:Guid}", deletePlatform).WithName("DeletePlatform")
                .RequireAuthorization();

            group.MapGet("getByName/{platformName}", getPlatformByName).WithName("GetPlatformByName")
                .RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> createPlatform(
                     [FromForm] CrPlatformReqDTO createPlatformDTO,
                     IPlatformsRepository repository,
                     IOutputCacheStore outputCacheStore,
                     IMapper mapper,
                     IFileStorage fileStorage)
        {
            var platform = mapper.Map<Platform>(createPlatformDTO);

            if (createPlatformDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createPlatformDTO.Icon);
                platform.Icon = url;
            }

            await repository.Create(platform);
            await outputCacheStore.EvictByTagAsync("get-platforms", default);

            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> updatePlatform(
            Guid platformId,
            [FromForm] UpPlatformReqDTO updatePlatformDTO,
            IPlatformsRepository repository,
            IMapper mapper,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {
            var platformToUpdate = await repository.GetById(platformId);

            if (platformToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updatePlatformDTO, platformToUpdate);

            if (updatePlatformDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(platformToUpdate.Icon, container, updatePlatformDTO.Icon);
                platformToUpdate.Icon = iconUrl;
            }

            await repository.Update(platformToUpdate);
            await outputCacheStore.EvictByTagAsync("get-platforms", default);

            return TypedResults.NoContent();
        }



        static async Task<Results<Ok<PlatformDTO>, NotFound>> getPlatformById(IPlatformsRepository repository, Guid platformId, IMapper mapper)
        {
            var platform = await repository.GetById(platformId);

            if (platform == null)
                return TypedResults.NotFound();

            var platformDTO = mapper.Map<PlatformDTO>(platform);

            return TypedResults.Ok(platformDTO);
        }

        static async Task<Ok<List<PlatformDTO>>> getAllPlatforms(IPlatformsRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var platforms = await repository.GetAll(pagination);
            var platformDTO = mapper.Map<List<PlatformDTO>>(platforms);
            return TypedResults.Ok(platformDTO);

        }

        static async Task<Ok<List<PlatformDTO>>> getPlatformByName(string platformName, IPlatformsRepository repository, IMapper mapper)
        {
            var platforms = await repository.GetByName(platformName);

            var platformDTO = mapper.Map<List<PlatformDTO>>(platforms);

            return TypedResults.Ok(platformDTO);

        }

        static async Task<Results<NoContent, NotFound>> deletePlatform(
            Guid platformId,
            IPlatformsRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {

            var regDB = await repository.GetById(platformId);

            if (regDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(platformId);
            await fileStorage.Delete(regDB.Icon, container);
            await outputCacheStore.EvictByTagAsync("get-platforms", default);
            return TypedResults.NoContent();
        }
    }
}

