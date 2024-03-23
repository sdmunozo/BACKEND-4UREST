using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Category;
using NetShip.DTOs.Common;
using NetShip.DTOs.DigitalMenu;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;
using System.Text.RegularExpressions;

namespace NetShip.Endpoints
{
    public static class EndpointsCategories
    {
        private static readonly string container = "categories";

        public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createCategory).WithName("CreateCategory").DisableAntiforgery().RequireAuthorization();

            group.MapPut("/update/{categoryId:Guid}", updateCategory).WithName("UpdateCategory").DisableAntiforgery().RequireAuthorization();

            group.MapGet("/getAll", getAllCategories).WithName("GetAllCategories")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-categories")).RequireAuthorization();

            group.MapGet("/getById/{categoryId:Guid}", getCategoryById).WithName("GetCategoryById").RequireAuthorization();

            group.MapDelete("/delete/{categoryId:Guid}", deleteCategory).WithName("DeleteCategory").RequireAuthorization();

            group.MapGet("getByName/{categoryName}", getCategoryByName).WithName("GetCategoryByName").RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> createCategory(
            [FromForm] CrCategoryReqDTO createCategoryDTO,
            ICategoriesRepository repository,
            IOutputCacheStore outputCacheStore,
            DigitalMenuService digitalMenuService,
            IMapper mapper,
            IFileStorage fileStorage)
        {
            var category = mapper.Map<Entities.Category>(createCategoryDTO);

            if (createCategoryDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createCategoryDTO.Icon);
                category.Icon = url;
            }

            await repository.Create(category);

            var categoryDigitalMenuDTO = mapper.Map<DTOs.DigitalMenu.Category>(category);

            await digitalMenuService.UpdateDigitalMenuJsonForCategory(category.CatalogId);

            await outputCacheStore.EvictByTagAsync("get-categories", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> updateCategory(
          Guid categoryId,
          [FromForm] UpCategoryReqDTO updateCategoryDTO,
          ICategoriesRepository repository,
          DigitalMenuService digitalMenuService,
          IMapper mapper,
          IFileStorage fileStorage,
          IOutputCacheStore outputCacheStore)
        {
            var categoryToUpdate = await repository.GetById(categoryId);

            if (categoryToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateCategoryDTO, categoryToUpdate);

            if (updateCategoryDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(categoryToUpdate.Icon, container, updateCategoryDTO.Icon);
                categoryToUpdate.Icon = iconUrl;
            }

            await repository.Update(categoryToUpdate);

            var categoryDigitalMenuDTO = mapper.Map<DTOs.DigitalMenu.Category>(categoryToUpdate);
            await digitalMenuService.UpdateDigitalMenuJsonForCategory(categoryToUpdate.CatalogId);


            await outputCacheStore.EvictByTagAsync("get-categories", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<Ok<CategoryDTO>, NotFound>> getCategoryById(ICategoriesRepository repository, Guid categoryId, IMapper mapper)
        {
            var category = await repository.GetById(categoryId);

            if (category == null)
                return TypedResults.NotFound();

            var categoryDTO = mapper.Map<CategoryDTO>(category);

            return TypedResults.Ok(categoryDTO);
        }

        static async Task<Ok<List<CategoryDTO>>> getAllCategories(ICategoriesRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var categories = await repository.GetAll(pagination);
            var categoryDTO = mapper.Map<List<CategoryDTO>>(categories);
            return TypedResults.Ok(categoryDTO);

        }

        static async Task<Ok<List<CategoryDTO>>> getCategoryByName(string categoryName, ICategoriesRepository repository, IMapper mapper)
        {
            var categories = await repository.GetByName(categoryName);

            var categoryDTO = mapper.Map<List<CategoryDTO>>(categories);

            return TypedResults.Ok(categoryDTO);

        }

        static async Task<Results<NoContent, NotFound>> deleteCategory(
                                                    Guid categoryId,
                                                    ICategoriesRepository categoriesRepository,
                                                    IBranchesRepository branchesRepository,
                                                    IFileStorage fileStorage,
                                                    DigitalMenuService digitalMenuService,
                                                    IOutputCacheStore outputCacheStore)
        {
            var categoryToDelete = await categoriesRepository.GetCategoryWithCatalogById(categoryId);

            if (categoryToDelete == null)
            {
                return TypedResults.NotFound();
            }

            if (categoryToDelete.Catalog == null)
            {
                return TypedResults.NotFound();
            }

            var branchId = categoryToDelete.Catalog.BranchId;

            await categoriesRepository.Delete(categoryId);

            if (!string.IsNullOrEmpty(categoryToDelete.Icon))
            {
                await fileStorage.Delete(categoryToDelete.Icon, container);
            }

            await digitalMenuService.RemoveCategoryFromDigitalMenuJson(categoryId, branchId);

            await outputCacheStore.EvictByTagAsync("get-categories", default);

            return TypedResults.NoContent();
        }

    }
}



/*
  static async Task<Results<NoContent, NotFound>> softDeleteCategory(Guid id, ICategoriesRepository repository, IOutputCacheStore outputCacheStore)
  {
      var category = await repository.GetById(id);

      if (category == null)
          return TypedResults.NotFound();

      category.Status = "Eliminado";

      await repository.Update(category);
      await outputCacheStore.EvictByTagAsync("get-categories", default);

      return TypedResults.NoContent();
  }*/

//group.MapDelete("softDelete/{id:Guid}", softDeleteCategory);