using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NetShip.DTOs.Auth;
using NetShip.Entities;
using NetShip.Filters;
using NetShip.Repositories;
using NetShip.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetShip.Endpoints
{
    public static class EndpointsUsers
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group) {
            group.MapPost("/register", Register).AddEndpointFilter<ValidationsFilter<RegisterNewUserAccountDTO>>();
            group.MapPost("/login", Login).AddEndpointFilter<ValidationsFilter<LoginRequestDTO>>();
            return group;
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<IEnumerable<IdentityError>>>> Register(
    RegisterNewUserAccountDTO registerNewUserAccountDTO,
    [FromServices] UserManager<ApplicationUser> userManager,
    [FromServices] IBrandsRepository brandsRepository,
    [FromServices] IBranchesRepository branchesRepository,
    IConfiguration configuration)
        {
            var user = new ApplicationUser
            {
                UserName = registerNewUserAccountDTO.UserEmail,
                Email = registerNewUserAccountDTO.UserEmail,
                FirstName = registerNewUserAccountDTO.UserFirstName,
                LastName = registerNewUserAccountDTO.UserLastName,
            };

            var result = await userManager.CreateAsync(user, registerNewUserAccountDTO.UserPassword);

            if (result.Succeeded)
            {
                var brand = new Brand
                {
                    Name = registerNewUserAccountDTO.BrandName,
                    User = user
                };

                var brandId = await brandsRepository.Create(brand);

                var branch = new Branch
                {
                    Name = registerNewUserAccountDTO.BranchName,
                    BrandId = brandId
                };

                var branchId = await branchesRepository.Create(branch);

                var userClaims = new List<Claim>
                {
                    new Claim("id", user.Id),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim("email", user.Email),
                    new Claim("brand_id", brandId.ToString()),
                    new Claim("branch_id", branchId.ToString()),
                };

                var responseCredentials = buildToken(userClaims, configuration);
                return TypedResults.Ok(responseCredentials);
            }
            else
            {
                return TypedResults.BadRequest(result.Errors);
            }
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<string>>> Login(
            LoginRequestDTO loginRequestDTO, [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromServices] UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.UserEmail);

            if (user == null)
            {
                return TypedResults.BadRequest("Login incorrecto");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginRequestDTO.UserPassword, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
        {
            new Claim("email", loginRequestDTO.UserEmail),
            // Agregar más claims según sea necesario
        };

                var authResult = buildToken(claims, configuration);
                return TypedResults.Ok(authResult);
            }
            else
            {
                return TypedResults.BadRequest("Login incorrecto");
            }
        }


        private static AuthenticationResponseDTO buildToken(List<Claim> claims, IConfiguration configuration)
        {
            var key = Keys.GetKey(configuration);
            var creds = new SigningCredentials(key.First(), SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiration = expiration,
            };
        }
    }
}
