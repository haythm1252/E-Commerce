using E_Commerce.Domain.Entities.Enums;
using E_Commerce.Infrastructure.Migrations;
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

        public IQueryable<Order> FilterdOrders(string filter)
        {
            IQueryable<Order> query = _context.Orders;
            if (filter == OrdersFilters.All.ToString())
                return query;
            else if (filter == OrdersFilters.Today.ToString())
                query = query.Where(o => o.CreatedAt.Date == DateTime.Today);
            else if (filter == OrdersFilters.Last30Days.ToString())
                query = query.Where(o => o.CreatedAt >= DateTime.Today.AddDays(-30));
            else
                query = query.Where(o => o.Status.ToString() == filter);

            return query;
        }
    
    }
}
