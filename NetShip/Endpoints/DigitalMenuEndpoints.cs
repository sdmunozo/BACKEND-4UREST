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
using NetShip.DTOs;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NetShip.Services;

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
                 .Produces(StatusCodes.Status404NotFound)
                 .WithTags("DigitalMenu");

            group.MapPost("/upload-digital-menu/{branchId:guid}", async (HttpContext httpContext, [FromRoute] Guid branchId, IFormFile file, ICatalogsRepository catalogRepository, ICategoriesRepository categoriesRepository, IProductsRepository productsRepository, IModifiersGroupsRepository modifiersGroupsRepository, IModifiersRepository modifiersRepository, IMapper mapper, DigitalMenuService digitalMenuService) =>
                await ProcessCsvFile(httpContext, file, branchId, catalogRepository, categoriesRepository, productsRepository, modifiersGroupsRepository, modifiersRepository, mapper, digitalMenuService))
                .WithName("UploadDigitalMenu")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .DisableAntiforgery()
                .WithTags("DigitalMenu");

            group.MapPost("/submit-feedback", SubmitFeedback)
                 .WithName("SubmitFeedback")
                 .Produces(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status400BadRequest)
                 .AllowAnonymous()
                 .WithTags("DigitalMenu");

            group.MapPost("/track-device", async (HttpContext httpContext, DeviceTrackingDTO deviceTrackingDto, IDigitalMenuRepository digitalMenuRepository, IMapper mapper) =>
                await TrackDevice(httpContext, deviceTrackingDto, digitalMenuRepository, mapper))
                .WithName("TrackDevice")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .AllowAnonymous()
                .WithTags("DigitalMenu");


            return group;
        }

        private static async Task<IResult> TrackDevice(HttpContext httpContext, DeviceTrackingDTO deviceTrackingDto, IDigitalMenuRepository digitalMenuRepository, IMapper mapper)
        {
            if (deviceTrackingDto == null)
            {
                return Results.BadRequest("El cuerpo de la solicitud es nulo.");
            }

            // Aquí podrías añadir validaciones adicionales a los datos recibidos si es necesario

            var deviceTracking = mapper.Map<DeviceTracking>(deviceTrackingDto);
            deviceTracking.CreatedAt = DateTime.UtcNow; // Establecer la fecha y hora de creación

            try
            {
                await digitalMenuRepository.TrackDeviceAsync(deviceTracking);
                return Results.Ok("Rastreo de dispositivo almacenado con éxito.");
            }
            catch (Exception ex)
            {
                // Considera lograr el error con detalles de `ex`
                return Results.Problem("Ocurrió un error al procesar el rastreo del dispositivo.");
            }
        }


        private static async Task<IResult> SubmitFeedback(HttpContext httpContext, FeedbackDTO feedbackDto, IDigitalMenuRepository digitalMenuRepository, IMapper mapper)
        {
            if (feedbackDto == null)
            {
                return Results.BadRequest("El cuerpo de la solicitud es nulo.");
            }

            var feedback = mapper.Map<Feedback>(feedbackDto);
            feedback.CreatedAt = DateTime.UtcNow;

            try
            {
                await digitalMenuRepository.CreateFeedbackAsync(feedback);
                return Results.Ok("Feedback enviado con éxito.");
            }
            catch (Exception)
            {
                // Considera lograr el error
                return Results.Problem("Ocurrió un error al procesar el feedback.");
            }
        }


        private static async Task<IResult> ProcessCsvFile(HttpContext httpContext, IFormFile file, Guid branchId, ICatalogsRepository catalogRepository, ICategoriesRepository categoriesRepository, IProductsRepository productsRepository, IModifiersGroupsRepository modifiersGroupsRepository, IModifiersRepository modifiersRepository, IMapper mapper, DigitalMenuService digitalMenuService)
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("Archivo CSV no proporcionado o vacío.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, csvConfig);

            var rows = new List<dynamic>();

            Guid? currentCatalogId = null, currentCategoryId = null, currentProductId = null, currentModifiersGroupId = null;
            string currentCatalogName = null, currentCategoryName = null, currentProductAlias = null, currentModifiersGroupAlias = null;
            
            decimal? pendingPriceForModifier = 0;
            InitModifierDTO pendingModifierDto = null;
            bool endProcess = false;

            while (csv.Read())
            {
                var rowType = csv.GetField(0);
                var description = csv.GetField(1);

                switch (rowType)
                {
                    case "CAG":
                        var catalogDto = new InitCatalogDTO { Name = description, BranchId = branchId };
                        var catalog = mapper.Map<Entities.Catalog>(catalogDto);
                        var createdCatalog = await catalogRepository.CreateCatalog(catalog);
                        currentCatalogId = createdCatalog.Id;
                        await digitalMenuService.UpdateDigitalMenuJsonForCatalog(branchId);
                        break;
                    case "CAT":
                        if (currentCatalogId == null) continue;
                        var categoryDto = new InitCategoryDTO { Name = description, CatalogId = currentCatalogId.Value };
                        var category = mapper.Map<Entities.Category>(categoryDto);
                        var createdCategoryId = await categoriesRepository.Create(category);
                        currentCategoryId = createdCategoryId;
                        await digitalMenuService.UpdateDigitalMenuJsonForCategory(currentCatalogId ?? Guid.NewGuid());
                        break;
                    case "PRO":
                        if (currentCategoryId == null) continue;
                        var productDto = new InitProductDTO { Alias = description, CategoryId = currentCategoryId.Value };
                        var product = mapper.Map<Entities.Product>(productDto);
                        var createdProductId = await productsRepository.Create(product);
                        currentProductId = createdProductId;
                        await digitalMenuService.UpdateDigitalMenuJsonForProduct(currentProductId ?? Guid.NewGuid());
                        break;
                    case "GMO":
                        if (currentProductId == null) continue;
                        var modifiersGroupDto = new InitModifiersGroupDTO { Alias = description, ProductId = currentProductId.Value };
                        var modifiersGroup = mapper.Map<Entities.ModifiersGroup>(modifiersGroupDto);
                        var createdModifiersGroupId = await modifiersGroupsRepository.Create(modifiersGroup);
                        currentModifiersGroupId = createdModifiersGroupId;
                        await digitalMenuService.UpdateDigitalMenuJsonForModifiersGroup(currentModifiersGroupId ?? Guid.NewGuid());
                        break;
                    case "MOD":
                        if (currentModifiersGroupId == null) continue;
                        pendingModifierDto = new InitModifierDTO { Alias = description, ModifiersGroupId = currentModifiersGroupId.Value };
                        break;
                    case "PRE":
                        if (pendingModifierDto != null && decimal.TryParse(description, out var price))
                        {
                            pendingModifierDto.Price = price.ToString();
                            var modifier = mapper.Map<Entities.Modifier>(pendingModifierDto);
                            var createdModifierId = await modifiersRepository.Create(modifier);
                            await digitalMenuService.UpdateDigitalMenuJsonForModifier(createdModifierId);
                            pendingModifierDto = null;

                            rows.Add(new
                            {
                                CatalogId = currentCatalogId,
                                CatalogName = currentCatalogName,
                                CategoryId = currentCategoryId,
                                CategoryName = currentCategoryName,
                                ProductId = currentProductId,
                                ProductAlias = currentProductAlias,
                                ModifierGroupId = currentModifiersGroupId,
                                ModifierGroupAlias = currentModifiersGroupAlias,
                                ModifierId = modifier.Id,
                                ModifierAlias = description
                            });
                        }
                        else
                        {
                         
                        }
                        break;
                    case "END":
                        endProcess = true;
                        Console.WriteLine("Proceso completado con éxito.");
                        break;
                    default:
                        break;
                }
            }

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(rows);
            await writer.FlushAsync();
            memoryStream.Position = 0;
            Console.WriteLine($" = = = = = = = = = = = = = = = = = = = = = = = END PROCESS = = = = = = = = = = = = = = = = = = = = = = = ");
            return Results.File(memoryStream, "text/csv", "DatosGenerados.csv");
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
