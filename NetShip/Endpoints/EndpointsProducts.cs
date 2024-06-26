﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Product;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsProducts
    {
        private static readonly string container = "products";

        public static RouteGroupBuilder MapProducts(this RouteGroupBuilder group)
        {

            group.MapPost("/create", createProduct).WithName("CreateProduct").DisableAntiforgery()
                .RequireAuthorization();

            group.MapPut("/update/{productId:Guid}", updateProduct).WithName("UpdateProduct").DisableAntiforgery()
                .RequireAuthorization();

            group.MapGet("/getAll", getAllProducts).WithName("GetAllProducts")
                .CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-products"))
                .RequireAuthorization();

            group.MapGet("/getById/{productId:Guid}", getProductById).WithName("GetProductById")
                .RequireAuthorization();

            group.MapDelete("/delete/{productId:Guid}", deleteProduct).WithName("DeleteProduct")
                .RequireAuthorization();

            group.MapGet("getByName/{productName}", getProductByName).WithName("GetProductByName")
                .RequireAuthorization();

            group.MapPut("/updateProductImage/{productId:Guid}", updateProductImage)
             .WithName("UpdateProductImage")
             .DisableAntiforgery()
             .RequireAuthorization();

            return group;
        }

        static async Task<Results<NoContent, NotFound>> updateProductImage(
               Guid productId,
               [FromForm] UpdateProductImageRequest updateProductImageRequest,
               IProductsRepository repository,
               IMapper mapper,
               IFileStorage fileStorage,
               IOutputCacheStore outputCacheStore,
               DigitalMenuService digitalMenuService)
        {
            var productToUpdate = await repository.GetById(productId);

            if (productToUpdate == null)
            {
                return TypedResults.NotFound();
            }

            if (updateProductImageRequest.Icon is not null)
            {
                var imageUrl = await fileStorage.Edit(productToUpdate.Icon, container, updateProductImageRequest.Icon);
                productToUpdate.Icon = imageUrl;

                await repository.Update(productToUpdate);
                await digitalMenuService.UpdateDigitalMenuJsonForProduct(productId);
                await outputCacheStore.EvictByTagAsync("get-products", default);

                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NoContent();
            }
        }

        static async Task<Results<NoContent, NotFound>> createProduct(
                         [FromForm] CrProductReqDTO createProductDTO,
                         IProductsRepository repository,
                         IOutputCacheStore outputCacheStore,
                         IMapper mapper,
                         IFileStorage fileStorage,
                         DigitalMenuService digitalMenuService)
        {
            var product = mapper.Map<Product>(createProductDTO);

            if (createProductDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createProductDTO.Icon);
                product.Icon = url;
            }

            await repository.Create(product);
            await digitalMenuService.UpdateDigitalMenuJsonForProduct(product.Id);
            await outputCacheStore.EvictByTagAsync("get-products", default);

            return TypedResults.NoContent();
        }



        static async Task<Results<NoContent, NotFound>> updateProduct(
                    Guid productId,
                    [FromForm] UpProductReqDTO updateProductDTO,
                    IProductsRepository repository,
                    IMapper mapper,
                    IFileStorage fileStorage,
                    IOutputCacheStore outputCacheStore,
                    DigitalMenuService digitalMenuService)
        {
            var productToUpdate = await repository.GetById(productId);

            if (productToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(updateProductDTO, productToUpdate);

            if (updateProductDTO.Icon is not null)
            {
                var iconUrl = await fileStorage.Edit(productToUpdate.Icon, container, updateProductDTO.Icon);
                productToUpdate.Icon = iconUrl;
            }

            await repository.Update(productToUpdate);
            await digitalMenuService.UpdateDigitalMenuJsonForProduct(productId);
            await outputCacheStore.EvictByTagAsync("get-products", default);

            return TypedResults.NoContent();
        }




        static async Task<Results<Ok<ProductDTO>, NotFound>> getProductById(IProductsRepository repository, Guid productId, IMapper mapper)
        {
            var product = await repository.GetById(productId);

            if (product == null)
                return TypedResults.NotFound();

            var productDTO = mapper.Map<ProductDTO>(product);

            return TypedResults.Ok(productDTO);
        }

        static async Task<Ok<List<ProductDTO>>> getAllProducts(IProductsRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var products = await repository.GetAll(pagination);
            var productDTO = mapper.Map<List<ProductDTO>>(products);
            return TypedResults.Ok(productDTO);

        }

        static async Task<Ok<List<ProductDTO>>> getProductByName(string productName, IProductsRepository repository, IMapper mapper)
        {
            var products = await repository.GetByName(productName);

            var productDTO = mapper.Map<List<ProductDTO>>(products);

            return TypedResults.Ok(productDTO);

        }

        static async Task<Results<NoContent, NotFound>> deleteProduct(
            Guid productId,
            IProductsRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore,
            DigitalMenuService digitalMenuService)
        {
            var productToDelete = await repository.GetById(productId);

            if (productToDelete == null)
            {
                return TypedResults.NotFound();
            }

            var branchIdNullable = await repository.GetBranchIdOfProduct(productId);

            if (!branchIdNullable.HasValue)
            {
                return TypedResults.NotFound();
            }

            Guid branchId = branchIdNullable.Value;

            await repository.Delete(productId);
            if (!string.IsNullOrEmpty(productToDelete.Icon))
            {
                await fileStorage.Delete(productToDelete.Icon, container);
            }
            await digitalMenuService.RemoveProductFromDigitalMenuJson(productId, branchId);
            await outputCacheStore.EvictByTagAsync("get-products", default);

            return TypedResults.NoContent();
        }


    }
}

