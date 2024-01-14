using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAll(PaginationDTO paginationDTO);
        Task<Product?> GetById(Guid id);
        Task<Guid> Create(Product product);
        Task<bool> Exist(Guid id);
        Task Update(Product product);
        Task Delete(Guid id);
        Task<List<Product>> GetByName(string name);
    }
}
