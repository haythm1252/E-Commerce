using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ShoppingCartItemReporsitory : BaseRepository<ShoppingCartItem>, IShoppingCartItemReporsitory
    {
        public ShoppingCartItemReporsitory(ApplicationDbContext context) : base(context)
        {
        }
    }
}
