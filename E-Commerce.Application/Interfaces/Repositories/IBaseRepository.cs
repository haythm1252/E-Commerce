using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T>? GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? criteria = null, params Expression<Func<T, object>>[] includes);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
