using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetShip.DTOs.Auth;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
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
            group.MapPost("/auth", AuthenticateToken).Produces<AuthenticationResponseDTO>(StatusCodes.Status200OK).Produces(StatusCodes.Status401Unauthorized);
            return group;
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<IEnumerable<IdentityError>>>> Register(
    RegisterNewUserAccountDTO registerNewUserAccountDTO,
    [FromServices] UserManager<ApplicationUser> userManager,
    [FromServices] IBrandsRepository brandsRepository,
    [FromServices] IBranchesRepository branchesRepository,
    IConfiguration configuration, ILoggerFactory loggerFactory)
        {

            var type = typeof(EndpointsUsers);
            var logger = loggerFactory.CreateLogger(type.FullName!);
            logger.LogInformation("registrando usuario");
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                foreach (var claim in userClaims)
                {
                    logger.LogInformation($"{claim.Type}: {claim.Value}");
                }

                var brandsList = new List<BrandDDLDTO>
                {
                    new BrandDDLDTO { Id = brandId, Name = registerNewUserAccountDTO.BrandName }
                };

                var responseCredentials = await buildToken(userClaims, user, configuration, brandsRepository, branchesRepository);

                return TypedResults.Ok(responseCredentials);
            }
            else
            {
                return TypedResults.BadRequest(result.Errors);
            }
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<string>>> Login(
            LoginRequestDTO loginRequestDTO, [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromServices] UserManager<ApplicationUser> userManager, IConfiguration configuration, ILoggerFactory loggerFactory, IBrandsRepository brandsRepository, IBranchesRepository branchesRepository)
        {
            var type = typeof(EndpointsUsers);
            var logger = loggerFactory.CreateLogger(type.FullName!);

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
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.Email, user.Email),
                    };

                foreach (var claim in claims)
                {
                    logger.LogInformation($"{claim.Type}: {claim.Value}");
                }


                var userBrands = await brandsRepository.GetBrandsByUserId(user.Id);
                var brandsList = userBrands.Select(b => new BrandDDLDTO { Id = b.Id, Name = b.Name }).ToList();

                var authResult = await buildToken(claims, user, configuration, brandsRepository, branchesRepository);

                return TypedResults.Ok(authResult);

            }
            else
            {
                return TypedResults.BadRequest("Login incorrecto");
            }
        }

        static async Task<IResult> AuthenticateToken(HttpRequest request, [FromServices] IConfiguration configuration, ILoggerFactory loggerFactory, IBrandsRepository brandsRepository, IBranchesRepository branchesRepository)
        {
            var type = typeof(EndpointsUsers);
            var logger = loggerFactory.CreateLogger(type.FullName!);

            if (request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var token = authHeader.ToString().Split(" ").Last();
                if (ValidateToken(token, configuration, out var tokenClaims))
                {
                    var userIdClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    var firstNameClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
                    var lastNameClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
                    var emailClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                    if (userIdClaim == null || firstNameClaim == null || lastNameClaim == null || emailClaim == null)
                    {
                        return Results.Json(new { msg = "Token no válido - Claim(s) faltante(s)" }, statusCode: StatusCodes.Status401Unauthorized);
                    }

                    var user = new ApplicationUser
                    {
                        Id = userIdClaim.Value,
                        FirstName = firstNameClaim.Value,
                        LastName = lastNameClaim.Value,
                        Email = emailClaim.Value,
                    };

                    var userBrands = await brandsRepository.GetBrandsByUserId(user.Id);
                    var brandsList = userBrands.Select(b => new BrandDDLDTO { Id = b.Id, Name = b.Name }).ToList();

                    var authResponse = await buildToken(new List<Claim> { userIdClaim, firstNameClaim, lastNameClaim, emailClaim }, user, configuration, brandsRepository, branchesRepository);

                    return Results.Ok(authResponse);
                }
                else
                {
                    return Results.Json(new { msg = "Token no válido" }, statusCode: StatusCodes.Status401Unauthorized);
                }
            }
            return Results.BadRequest("Authorization header is missing.");
        }



        private static bool ValidateToken(string token, IConfiguration configuration, out List<Claim> tokenClaims)
        {
            tokenClaims = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Keys.GetKey(configuration).First(),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                tokenClaims = principal.Claims.ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static async Task<AuthenticationResponseDTO> buildToken(List<Claim> claims, ApplicationUser user, IConfiguration configuration, IBrandsRepository brandsRepository, IBranchesRepository branchesRepository)
        {
            var key = Keys.GetKey(configuration);
            var creds = new SigningCredentials(key.First(), SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var brand = await brandsRepository.GetFirstBrandByUserId(user.Id);
            var branch = brand != null ? await branchesRepository.GetFirstByBrandId(brand.Id) : null;

            var userResponse = new UserResponseDTO
            {
                UserId = user.Id,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserEmail = user.Email,
            };

            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiration = expiration,
                User = userResponse,
                Brand = brand, 
                Branch = branch
            };
        }

        /*
        private static AuthenticationResponseDTO buildToken(List<Claim> claims, ApplicationUser user, IConfiguration configuration, IBrandsRepository brandsRepository, IBranchesRepository branchesRepository)
        {
            var key = Keys.GetKey(configuration);
            var creds = new SigningCredentials(key.First(), SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var userResponse = new UserResponseDTO
            {
                UserId = user.Id,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserEmail = user.Email,
            };

            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiration = expiration,
                User = userResponse,
                Brands = brands
            };
        } 

        */


    }
}
