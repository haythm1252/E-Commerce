using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product>? GetProductWithCategoryProductsAsync(int id);
    }
}
