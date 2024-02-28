using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using NetShip.DTOs.Auth;
using NetShip.DTOs.Brand;
using NetShip.Repositories;

namespace NetShip.Endpoints
{
    public static class EndpointsBrands
    {
        public static RouteGroupBuilder MapBrands(this RouteGroupBuilder group)
        {
            return group;
        }
        /*
        static async Task<Results‹Created‹BrandDTO>, NotFound>> Register(int userId,
        CreateBrandDTO createBrandDTO, IBrandsRepository brandsRepository, IUserRepository userRepository)
        {
            if (!await IUserRepository.Existe(peliculaId))
            {
                return TypedResults.NotFound();
                var comentario = mapper Map‹Comentario› (crearComentarioDTO); comentario.PeliculaId = peliculaId;
                var id = await repositorioComentarios.Crear(comentario);
            }


        } */
    }
}
