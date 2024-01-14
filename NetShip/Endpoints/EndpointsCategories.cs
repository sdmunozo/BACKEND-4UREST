using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Category;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsCategories
    {
        private static readonly string container = "categories";

        public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
        {
            group.MapGet("/", getCategories).CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-categories"));
            group.MapGet("/{id:Guid}", getCategory);
            group.MapPost("/", createCategory).DisableAntiforgery();
            group.MapPut("/{id:Guid}", updateCategory).DisableAntiforgery();
            group.MapDelete("/{id:Guid}", deleteCategory);
            group.MapGet("getByName/{name}", getCategoryByName);

            return group;
        }


        static async Task<Created<CategoryDTO>> createCategory([FromForm] CreateCategoryDTO createCategoryDTO, 
            ICategoriesRepository repository,  IOutputCacheStore outputCacheStore,
            IMapper mapper, IFileStorage fileStorage)
        {
            var category = mapper.Map<Category>(createCategoryDTO);

            if (createCategoryDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createCategoryDTO.Icon);
                category.Icon = url;
            }

            var id = await repository.Create(category);
            await outputCacheStore.EvictByTagAsync("get-categories", default);

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Created($"/category/{id}", categoryDTO);
        }

        static async Task<Results<Ok<CategoryDTO>, NotFound>> getCategory(ICategoriesRepository repository, Guid id, IMapper mapper)
        {
            var category = await repository.GetById(id);

            if (category == null)
                return TypedResults.NotFound();

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Ok(categoryDTO);
        }

        static async Task<Ok<List<CategoryDTO>>> getCategories(ICategoriesRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var categories = await repository.GetAll(pagination);
            var categoryDTO = mapper.Map<List<CategoryDTO>>(categories);
            return TypedResults.Ok(categoryDTO);

        }

        static async Task<Ok<List<CategoryDTO>>> getCategoryByName(string name, ICategoriesRepository repository, IMapper mapper)
        {
            var categories = await repository.GetByName(name);

            var categoryDTO = mapper.Map<List<CategoryDTO>>(categories);

            return TypedResults.Ok(categoryDTO);

        }


        static async Task<Results<NoContent, NotFound>> updateCategory(
            Guid id,
            [FromForm] CreateCategoryDTO createCategoryDTO, 
            ICategoriesRepository repository, 
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore, 
            IMapper mapper)
        {
            var regDB = await repository.GetById(id);

            if (regDB is null)
                return TypedResults.NotFound();

            var registerToUpdate = mapper.Map<Category>(createCategoryDTO);
            registerToUpdate.Id = id;
            registerToUpdate.Icon = regDB.Icon;

            if (createCategoryDTO.Icon is not null)
            {
                var url = await fileStorage.Edit(registerToUpdate.Icon, container, createCategoryDTO.Icon);
                registerToUpdate.Icon = url;
            }

            await repository.Update(registerToUpdate);
            await outputCacheStore.EvictByTagAsync("get-categories", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> deleteCategory(Guid id, ICategoriesRepository repository, IOutputCacheStore outputCacheStore)
        {
            var category = await repository.GetById(id);

            if (category == null)
                return TypedResults.NotFound();

            category.Status = "Eliminado";

            await repository.Update(category);
            await outputCacheStore.EvictByTagAsync("get-categories", default);

            return TypedResults.NoContent();
        }



        /*
         * 
         * Endpoint para eliminar permanentemente:
         * 
        
        static async Task<Results<NoContent, NotFound>> deleteCategory(Guid id, ICategoriesRepository repository, IOutputCacheStore outputCacheStore)
        {
            var exist = await repository.Exist(id);

            if (!exist)
                return TypedResults.NotFound();
            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("get-categories", default);
            return TypedResults.NoContent();
        }

        */
    }
}

