
namespace E_Commerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IShoppingcartRepository ShoppingCarts { get; private set; }
        public IShoppingCartItemReporsitory ShoppingCartItems { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IOrderItemRepository OrderItems { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            ShoppingCarts = new ShoppingCartRepository(_context);
            ShoppingCartItems = new ShoppingCartItemReporsitory(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new OrderItemRepository(_context);
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
