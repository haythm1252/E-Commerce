namespace E_Commerce.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

    }
}
