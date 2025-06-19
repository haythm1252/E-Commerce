using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Order> GetOrderWithProducts(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.Id == id);
        }
        public async Task<List<Order>> GetAllOrderWithProducts(Expression<Func<Order, bool>> criteria)
        {
            return await _context.Orders.Where(criteria)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }
    }
}
