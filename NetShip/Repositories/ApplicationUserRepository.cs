using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.Common;
using NetShip.Entities;
using NetShip.Utilities;

namespace NetShip.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly HttpContext httpContext;

        public ApplicationUserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<string> Create(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return user.Id;
            }
            else
            {
                throw new Exception("No se pudo crear el usuario");
            }
        }

        public async Task<bool> Exist(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user != null;
        }

        public async Task<List<ApplicationUser>> GetAll(PaginationDTO paginationDTO)
        {
            var queryable = userManager.Users.AsQueryable();
            await httpContext.InsertPaginationParametersInHeader(queryable);
            return await userManager.Users.Paginate(paginationDTO).OrderBy(x => x.Email).ToListAsync();
        }

        public async Task<ApplicationUser?> GetById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task Update(ApplicationUser user)
        {
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("No se pudo actualizar el usuario");
            }
        }

        public async Task Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("No se pudo eliminar el usuario");
                }
            }
        }

        public async Task<List<ApplicationUser>> GetByName(string name)
        {
            return await userManager.Users.Where(u => u.UserName.Contains(name) || u.Email.Contains(name)).OrderBy(u => u.UserName).ToListAsync();
        }

    }
}
