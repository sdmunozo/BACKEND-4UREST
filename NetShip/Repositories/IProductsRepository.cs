using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product?> GetProduct(Guid id);
        Task<Guid> Create(Product product);
        Task<bool> Exist(Guid id);
        Task Update(Product product);
        Task Delete(Guid id);
    }
}
