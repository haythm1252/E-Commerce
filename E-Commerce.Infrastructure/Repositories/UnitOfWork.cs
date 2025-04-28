
namespace E_Commerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
