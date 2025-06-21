
using E_Commerce.Application.Common;

namespace E_Commerce.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task<T>? GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<T>? FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbset;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? criteria = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbset;

            if (criteria != null)
                query = query.Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? criteria = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbset;
            if (criteria != null)
                query = query.Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            var totalCount = await query.CountAsync();
            return new PagedResult<T>
            {
                Items = await query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
