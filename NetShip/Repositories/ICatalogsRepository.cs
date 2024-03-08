using NetShip.DTOs.CatalogDTOs;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface ICatalogsRepository
    {
        Task<ListOfCatalogsDTO> GetCatalogsByBranchId(Guid branchId);
        Task<Catalog> CreateCatalog(Catalog catalog);
        Task UpdateCatalog(Catalog catalog);
        Task<Catalog?> GetById(Guid id);
        Task DeleteCatalog(Guid id);
    }
}
