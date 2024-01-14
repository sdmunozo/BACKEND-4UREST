using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Category;
using NetShip.Entities;
using NetShip.Repositories;

namespace NetShip.Endpoints
{
    public static class EndpointsCategories
    {
        public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
        {
            group.MapGet("/", getCategories).CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-categories"));
            group.MapGet("/{id:Guid}", getCategory);
            group.MapPost("/", createCategory);
            group.MapPut("/{id:Guid}", updateCategory);
            group.MapDelete("/{id:Guid}", deleteCategory);

            return group;
        }


        static async Task<Created<CategoryDTO>> createCategory(CreateCategoryDTO createCategoryDTO, 
            ICategoriesRepository repository, 
            IOutputCacheStore outputCacheStore,
            IMapper mapper)
        {
            var category = mapper.Map<Category>(createCategoryDTO);
            var id = await repository.Create(category);
            await outputCacheStore.EvictByTagAsync("get-categories", default);

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Created($"/category/{id}", categoryDTO);
        }

        static async Task<Results<Ok<CategoryDTO>, NotFound>> getCategory(ICategoriesRepository repository, Guid id, IMapper mapper)
        {
            var category = await repository.GetCategory(id);

            if (category == null)
                return TypedResults.NotFound();

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Ok(categoryDTO);
        }

        static async Task<Ok<List<CategoryDTO>>> getCategories(ICategoriesRepository repository, IMapper mapper)
        {
            var categories = await repository.GetCategories();

            var categoryDTO = mapper.Map<List<CategoryDTO>>(categories);

            return TypedResults.Ok(categoryDTO);

        }

        static async Task<Results<NoContent, NotFound>> updateCategory(Guid id, CreateCategoryDTO createCategoryDTO, ICategoriesRepository repository, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var exist = await repository.Exist(id);

            if (!exist)
                return TypedResults.NotFound();

            var category = mapper.Map<Category>(createCategoryDTO);
            category.Id = id;

            await repository.Update(category);
            await outputCacheStore.EvictByTagAsync("get-categories", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> deleteCategory(Guid id, ICategoriesRepository repository, IOutputCacheStore outputCacheStore)
        {
            var category = await repository.GetCategory(id);

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

