using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetOrderWithProducts(int id);
        Task<List<Order>> GetAllOrderWithProducts(Expression<Func<Order, bool>> criteria);
        IQueryable<Order> FilterdOrders(string filter);
    }

}
