using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? criteria=null, params Expression<Func<Category, object>>[] includes);
    }
}
