using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetShip.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NetShip.Entities;
using NetShip.DTOs.DigitalMenu;
using System;

namespace NetShip.Endpoints
{
    public static class DigitalMenuEndpoints
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public static RouteGroupBuilder MapDigitalMenu(this RouteGroupBuilder group)
        {
            group.MapGet("/get-digital-menu/{normalizedDigitalMenu}", async (HttpContext httpContext, string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper) =>
                await GetBranchCatalog(httpContext, normalizedDigitalMenu, branchRepository, mapper))
                 .WithName("GetBranchCatalog")
                 .Produces<BranchCatalogResponse>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status404NotFound);

            return group;
        }

        private static async Task<IResult> GetBranchCatalog(HttpContext httpContext, string normalizedDigitalMenu, IBranchesRepository branchRepository, IMapper mapper)
        {
            await semaphore.WaitAsync();
            try
            {
                var branch = await branchRepository.GetByNormalizedDigitalMenu(normalizedDigitalMenu);
                if (branch == null)
                {
                    return Results.NotFound("Sucursal no encontrada con el enlace de menú digital proporcionado.");
                }

                var digitalMenu = await branchRepository.GetDigitalMenuByBranchId(branch.Id);
                if (digitalMenu == null)
                {
                    return Results.NotFound("Menú digital no encontrado para la sucursal especificada.");
                }

                var digitalMenuResponse = mapper.Map<BranchCatalogResponse>(digitalMenu);

                return Results.Ok(digitalMenuResponse);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
