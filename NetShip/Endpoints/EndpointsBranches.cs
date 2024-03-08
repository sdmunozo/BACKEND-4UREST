using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Branch;
using NetShip.Repositories;
using System;

namespace NetShip.Endpoints
{
    public static class EndpointsBranches
    {
        public static RouteGroupBuilder MapBranches(this RouteGroupBuilder group)
        {
            group.MapGet("/byBrand/{brandId:Guid}", getBranchesByBrandId)
                .WithName("GetBranchesByBrandId")
                .Produces<List<BranchDTO>>(StatusCodes.Status200OK)
                .RequireAuthorization();

            return group;
        }

        private static async Task<IResult> getBranchesByBrandId(Guid brandId, IBranchesRepository repository, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            var branches = await repository.GetByBrandId(brandId);
            if (branches is null || branches.Count == 0)
            {
                return TypedResults.NotFound();
            }

            var branchDTOs = mapper.Map<List<BranchDTO>>(branches);
            return TypedResults.Ok(branchDTOs);
        }

    }
}
