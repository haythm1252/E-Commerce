using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingcartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext Context) : base(Context)
        {

        }

        public async Task<ShoppingCart> GetCartWithProducts(int id)
        {
            return await _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(sc => sc.Product)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
