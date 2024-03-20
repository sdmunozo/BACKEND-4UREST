using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Modifier;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsModifiers
    {
        private static readonly string container = "modifiers";

        public static RouteGroupBuilder MapModifiers(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createModifier).WithName("CreateModifier").DisableAntiforgery()
                .RequireAuthorization();

            group.MapPut("/update/{modifierId:Guid}", updateModifier).WithName("UpdateModifier").DisableAntiforgery()
                .RequireAuthorization();

            group.MapGet("/getAll", getAllModifiers).WithName("GetAllModifiers")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-modifiers"))
                .RequireAuthorization();

            group.MapGet("/getById/{modifierId:Guid}", getModifierById).WithName("GetModifierById")
                .RequireAuthorization();

            group.MapDelete("/delete/{modifierId:Guid}", deleteModifier).WithName("DeleteModifier")
                .RequireAuthorization();

            group.MapGet("getByName/{modifierName}", getModifierByName).WithName("GetModifierByName")
                .RequireAuthorization();

            group.MapPost("setPlatform/{modifierId:Guid}", SetPlatform).WithName("SetPlatformModifier")
                .RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> createModifier(
                     [FromForm] CrModifierReqDTO createModifierDTO,
                     IModifiersRepository repository,
                     IOutputCacheStore outputCacheStore,
                     IMapper mapper,
                     IFileStorage fileStorage)
        {
            var modifier = mapper.Map<Modifier>(createModifierDTO);

            if (createModifierDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createModifierDTO.Icon);
                modifier.Icon = url;
            }

            await repository.Create(modifier);
            await outputCacheStore.EvictByTagAsync("get-modifiers", default);

            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> updateModifier(
            Guid modifierId,
            [FromForm] UpModifierReqDTO updateModifierDTO,
            IModifiersRepository repository,
            IMapper mapper,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {
            var modifierToUpdate = await repository.GetById(modifierId);

            if (modifierToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateModifierDTO, modifierToUpdate);

            if (updateModifierDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(modifierToUpdate.Icon, container, updateModifierDTO.Icon);
                modifierToUpdate.Icon = iconUrl;
            }

            await repository.Update(modifierToUpdate);
            await outputCacheStore.EvictByTagAsync("get-modifiers", default);

            return TypedResults.NoContent();
        }



        static async Task<Results<Ok<ModifierDTO>, NotFound>> getModifierById(IModifiersRepository repository, Guid modifierId, IMapper mapper)
        {
            var modifier = await repository.GetById(modifierId);

            if (modifier == null)
                return TypedResults.NotFound();

            var modifierDTO = mapper.Map<ModifierDTO>(modifier);

            return TypedResults.Ok(modifierDTO);
        }

        static async Task<Ok<List<ModifierDTO>>> getAllModifiers(IModifiersRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var modifiers = await repository.GetAll(pagination);
            var modifierDTO = mapper.Map<List<ModifierDTO>>(modifiers);
            return TypedResults.Ok(modifierDTO);

        }

        static async Task<Ok<List<ModifierDTO>>> getModifierByName(string modifierName, IModifiersRepository repository, IMapper mapper)
        {
            var modifiers = await repository.GetByName(modifierName);

            var modifierDTO = mapper.Map<List<ModifierDTO>>(modifiers);

            return TypedResults.Ok(modifierDTO);

        }

        static async Task<Results<NoContent, NotFound>> deleteModifier(
            Guid modifierId,
            IModifiersRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore)
        {

            var regDB = await repository.GetById(modifierId);

            if (regDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(modifierId);
            await fileStorage.Delete(regDB.Icon, container);
            await outputCacheStore.EvictByTagAsync("get-modifiers", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> SetPlatform(Guid modifierId, List<SetPlatformModifier> setModifiers,
                                IModifiersRepository modifiersRepository, IPlatformsRepository platformsRepository, IMapper mapper)
        {
            if (!await modifiersRepository.Exist(modifierId))
            {
                return TypedResults.NotFound();
            }

            var existingPlatforms = new List<Guid>();
            var platformsIds = setModifiers.Select(x => x.PlatformId).ToList();

            if (setModifiers.Count != 0)
            {
                existingPlatforms = await platformsRepository.CheckAll(platformsIds);
            }

            if (existingPlatforms.Count != setModifiers.Count)
            {
                var noExistingPlatforms = platformsIds.Except(existingPlatforms);

                return TypedResults.BadRequest($"Las plataformas de id {string.Join(",", noExistingPlatforms)} no existen.");
            }

            var platforms = mapper.Map<List<PricePerModifierPerPlatform>>(setModifiers);

            await modifiersRepository.SetPlatforms(modifierId, platforms);
            return TypedResults.NoContent();
        }

    }
}

