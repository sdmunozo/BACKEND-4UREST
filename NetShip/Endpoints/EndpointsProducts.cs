using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Common;
using NetShip.DTOs.Product;
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
            group.MapGet("/", getProducts).CacheOutput(c => c.Expire(TimeSpan.FromDays(30)).Tag("get-products"));
            group.MapGet("/{id:Guid}", getProduct);
            group.MapPost("/", createProduct).DisableAntiforgery();
            group.MapPut("/{id:Guid}", updateProduct).DisableAntiforgery();
            group.MapDelete("/{id:Guid}", deleteProduct); 
            group.MapDelete("softDelete/{id:Guid}", softDeleteProduct);
            group.MapGet("getByName/{name}", getProductByName);

            return group;
        }

        static async Task<Created<ProductDTO>> createProduct([FromForm] CreateProductDTO createProductDTO,
            IProductsRepository repository, IOutputCacheStore outputCacheStore,
            IMapper mapper, IFileStorage fileStorage)
        {
            var product = mapper.Map<Product>(createProductDTO);

            if (createProductDTO.Icon is not null)
            {
                var url = await fileStorage.Upload(container, createProductDTO.Icon);
                product.Icon = url;
            }

            var id = await repository.Create(product);
            await outputCacheStore.EvictByTagAsync("get-products", default);

            var productDTO = mapper.Map<ProductDTO>(product);

            return TypedResults.Created($"/product/{id}", productDTO);
        }

        static async Task<Results<Ok<ProductDTO>, NotFound>> getProduct(IProductsRepository repository, Guid id, IMapper mapper)
        {
            var product = await repository.GetById(id);

            if (product == null)
                return TypedResults.NotFound();

            var productDTO = mapper.Map<ProductDTO>(product);

            return TypedResults.Ok(productDTO);
        }

        static async Task<Ok<List<ProductDTO>>> getProducts(IProductsRepository repository, IMapper mapper, int page, int recordsPerPage)
        {
            var pagination = new PaginationDTO { Page = page , RecordsPerPage = recordsPerPage};
            var products = await repository.GetAll(pagination);
            var productDTO = mapper.Map<List<ProductDTO>>(products);
            return TypedResults.Ok(productDTO);
        }

        static async Task<Ok<List<ProductDTO>>> getProductByName(string name, IProductsRepository repository, IMapper mapper)
        {
            var products = await repository.GetByName(name);

            var productDTO = mapper.Map<List<ProductDTO>>(products);

            return TypedResults.Ok(productDTO);

        }

        static async Task<Results<NoContent, NotFound>> updateProduct(
            Guid id,
            [FromForm] CreateProductDTO createProductDTO,
            IProductsRepository repository,
            IFileStorage fileStorage,
            IOutputCacheStore outputCacheStore,
            IMapper mapper)
        {
            // Obtén la entidad existente del contexto sin crear una nueva instancia.
            var registerToUpdate = await repository.GetById(id);

            if (registerToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            // Actualiza las propiedades de la entidad con los valores del DTO.
            mapper.Map(createProductDTO, registerToUpdate);

            if (createProductDTO.Icon is not null)
            {
                var url = await fileStorage.Edit(registerToUpdate.Icon, container, createProductDTO.Icon);
                registerToUpdate.Icon = url;
            }

            // Marca la entidad como modificada en el contexto.
            await repository.Update(registerToUpdate);

            await outputCacheStore.EvictByTagAsync("get-products", default);
            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> softDeleteProduct(Guid id, IProductsRepository repository, IOutputCacheStore outputCacheStore)
        {
            var product = await repository.GetById(id);

            if (product == null)
                return TypedResults.NotFound();

            product.Status = "Eliminado";

            await repository.Update(product);
            await outputCacheStore.EvictByTagAsync("get-products", default);

            return TypedResults.NoContent();
        }


        static async Task<Results<NoContent, NotFound>> deleteProduct(
                    Guid id,
                    IProductsRepository repository,
                    IFileStorage fileStorage,
                    IOutputCacheStore outputCacheStore)
        {

            var regDB = await repository.GetById(id);

            if(regDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await fileStorage.Delete(regDB.Icon, container);
            await outputCacheStore.EvictByTagAsync("get-products", default);
            return TypedResults.NoContent();
        }

    }

}
