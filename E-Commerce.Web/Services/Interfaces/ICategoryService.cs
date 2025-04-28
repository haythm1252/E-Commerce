using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category>? GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? criteria = null, params Expression<Func<Category, object>>[] includes);
        Task AddAsync(CategoryVM category);
        Task<bool> EditAsync(CategoryVM category);
    }
}
