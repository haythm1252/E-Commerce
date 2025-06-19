using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product>? GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? criteria = null, params Expression<Func<Product, object>>[] includes);
        Task<Product>? Details(int id);
        Task<List<Product>> SearchById(int id);
        Task<bool> AddAsync(CreateProductVM product);
        Task<bool> EditAsync(EditProductVM product);
        Task<bool> DeleteAsync(int id);
    }
}
