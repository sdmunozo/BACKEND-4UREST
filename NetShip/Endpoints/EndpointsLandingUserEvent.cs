using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.LandingUserEvent;
using NetShip.Entities;
using NetShip.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetShip.Endpoints
{
    public static class EndpointsLandingUserEvent
    {
        private static readonly string container = "events";

        public static RouteGroupBuilder MapLandingUserEvents(this RouteGroupBuilder group)
        {
            group.MapPost("/create", createLandingUserEvent).WithName("CreateLandingUserEvent");//.RequireAuthorization();
            group.MapGet("/getAll", getAllLandingUserEvents).WithName("GetAllLandingUserEvents");//.RequireAuthorization();

            return group;
        }

        static async Task<IResult> createLandingUserEvent(
        [FromBody] LandingUserEventDTO landingUserEventDTO,
        ILandingUserEventRepository landingUserEventRepository,
        IMapper mapper)
        {
            // Validación básica de los datos de entrada
            if (landingUserEventDTO == null)
            {
                return Results.BadRequest("Invalid event data provided.");
            }

            try
            {
                // Mapeo del DTO a la entidad
                var landingUserEvent = mapper.Map<LandingUserEvent>(landingUserEventDTO);

                // Creación del evento en la base de datos
                var createdEventId = await landingUserEventRepository.Create(landingUserEvent);

                // Respuesta exitosa con el ID del evento creado
                return Results.Created($"/getById/{createdEventId}", createdEventId);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return Results.Problem($"An error occurred while creating the event: {ex.Message}");
            }
        }

        static async Task<IResult> getAllLandingUserEvents(
    ILandingUserEventRepository landingUserEventRepository,
    IMapper mapper)
        {
            try
            {
                // Obtención de todos los eventos desde el repositorio
                var events = await landingUserEventRepository.GetAll();

                // Si no hay eventos, devuelve una respuesta vacía
                if (events == null || events.Count == 0)
                {
                    return Results.Ok(new List<LandingUserEventDTO>());
                }

                // Mapeo de las entidades a DTOs
                var eventDTOs = mapper.Map<List<LandingUserEventDTO>>(events);

                // Devuelve la lista de eventos como respuesta
                return Results.Ok(eventDTOs);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales para evitar la propagación de errores al cliente
                return Results.Problem($"An error occurred while retrieving events: {ex.Message}");
            }
        }


    }
}
