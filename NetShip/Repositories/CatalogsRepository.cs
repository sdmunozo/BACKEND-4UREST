using Microsoft.EntityFrameworkCore;
using NetShip.DTOs.CatalogDTOs;
using NetShip.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetShip.Repositories
{
    public class CatalogsRepository : ICatalogsRepository
    {
        private readonly ApplicationDbContext _context;

        public CatalogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ListOfCatalogsDTO> GetCatalogsByBranchName(string branchName)
        {
            var catalogs = await _context.Catalogs
                .Where(c => c.Branch.Name.Contains(branchName))
                .ToListAsync();

            var catalogDTOs = catalogs.Select(c => new CatalogDetailsDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                IsScheduleActive = c.IsScheduleActive,
                Sort = c.Sort,
                Icon = c.Icon
            }).ToList();

            var result = new ListOfCatalogsDTO
            {
                Total = catalogDTOs.Count,
                Catalogs = catalogDTOs
            };

            return result;
        }

        public async Task<Catalog> CreateCatalog(Catalog catalog)
        {
            _context.Add(catalog);
            await _context.SaveChangesAsync();
            return catalog; 
        }



        public async Task DeleteCatalog(Guid id)
        {
            var catalog = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == id);
            if (catalog != null)
            {
                _context.Catalogs.Remove(catalog);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Catalog?> GetById(Guid id)
        {
            return await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<ListOfCatalogsDTO> GetCatalogsByBranchId(Guid branchId)
        {
            var catalogs = await _context.Catalogs
                .Where(c => c.BranchId == branchId)
                .ToListAsync();

            var catalogDTOs = catalogs.Select(c => new CatalogDetailsDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                IsScheduleActive = c.IsScheduleActive,
                Sort = c.Sort,
                Icon = c.Icon
            }).ToList();

            var result = new ListOfCatalogsDTO
            {
                Total = catalogDTOs.Count,
                Catalogs = catalogDTOs
            };

            return result;
        }

        public async Task UpdateCatalog(Catalog catalog)
        {
            _context.Update(catalog);
            await _context.SaveChangesAsync();
        }

       

    }
}
