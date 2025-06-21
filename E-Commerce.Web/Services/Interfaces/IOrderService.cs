using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IOrderService
    {
        PagedResult<OrderVM> GetFilterdOrders(string filter, int pageNumber, int pageSize);
        Task<PagedResult<OrderVM>> SearchById(int id);
        Task<UserDetailsVM> GetUserWithOrders(string userId);
        Task<OrderVM> GetOrderDetails(int id);
        Task<Order> CreateOrderAsync(int cartId);
        Task<CheckoutVM> CheckoutAsync(int orderId);
        Task<OrderVM> GetOrderVMAsync(Order order);
        Task<bool> ConfirmOrderAsync(int id);
        Task<(int success, int faild)> ConfirmAllOrdersAsync();
    }
}
