


namespace E_Commerce.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
        public async Task<Product>? GetProductWithCategoryProductsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.Products)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
