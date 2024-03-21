using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Brand;
using NetShip.Repositories;
using NetShip.Services;

namespace NetShip.Endpoints
{
    public static class EndpointsBrands
    {
        private static readonly string container = "brands";

        public static RouteGroupBuilder MapBrands(this RouteGroupBuilder group)
        {
            group.MapPut("/update/{brandId:Guid}", updateBrand)
                 .WithName("UpdateBrand")
                 .DisableAntiforgery(); // Opcional, según tu configuración y necesidades de seguridad

            return group;
        }

        static async Task<Results<NoContent, NotFound>> updateBrand(
            Guid brandId,
            [FromForm] UpBrandReqDTO brandDto,
            IBrandsRepository repository,
            IMapper mapper,
            IFileStorage fileStorage,
            [FromServices] DigitalMenuService digitalMenuService,
            IOutputCacheStore outputCacheStore)
        {
            var brandToUpdate = await repository.GetById(brandId);

            if (brandToUpdate is null)
            {
                return TypedResults.NotFound();
            }

            // Actualiza las propiedades de la marca con los valores del DTO
            mapper.Map(brandDto, brandToUpdate);

            // Procesamiento del logo si se incluye
            if (brandDto.Logo is not null)
            {
                var logoUrl = await fileStorage.Edit(brandToUpdate.Logo, container, brandDto.Logo);
                brandToUpdate.Logo = logoUrl;
            }

            // Procesamiento del fondo de catálogos si se incluye
            if (brandDto.CatalogsBackground is not null)
            {
                var catalogsBackgroundUrl = await fileStorage.Edit(brandToUpdate.CatalogsBackground, container, brandDto.CatalogsBackground);
                brandToUpdate.CatalogsBackground = catalogsBackgroundUrl;
            }

            // Actualiza el JSON del menú digital para todas las sucursales de esta marca
            await digitalMenuService.UpdateDigitalMenuJsonForBrand(brandId);

            return TypedResults.NoContent();

        }
    }
}


