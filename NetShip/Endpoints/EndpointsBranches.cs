using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Branch;
using NetShip.Repositories;
using NetShip.Services;
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

            group.MapPost("/generateBranchLink/{branchId:Guid}", generateBranchLink)
                .WithName("GenerateBranchLink")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
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

        private static async Task<IResult> generateBranchLink(Guid branchId, IBranchesRepository branchesRepository, IBrandsRepository brandsRepository, QrCodeService qrCodeService, HttpContext httpContext)
        {

            var branch = await branchesRepository.GetById(branchId);
            if (branch == null)
            {
                return TypedResults.NotFound();
            }


            var brand = await brandsRepository.GetById(branch.BrandId);
            if (brand == null)
            {
                return TypedResults.NotFound();
            }

            var domain = "https://4urest.com/";
            var digitalMenuLink = $"{domain}{brand.UrlNormalizedName}-{branch.UrlNormalizedName}";
            var normalizedDigitalMenu = $"{brand.UrlNormalizedName}-{branch.UrlNormalizedName}";

            branch.DigitalMenuLink = digitalMenuLink;
            branch.NormalizedDigitalMenu = normalizedDigitalMenu;

            var qrCodePath = qrCodeService.GenerateQrCode(digitalMenuLink, $"wwwroot/qrcodes/{branchId}.png");

            branch.QrCodePath = qrCodePath;

            await branchesRepository.Update(branch);

            return TypedResults.Ok(new { branch.DigitalMenuLink, branch.QrCodePath });
        }




    }
}
