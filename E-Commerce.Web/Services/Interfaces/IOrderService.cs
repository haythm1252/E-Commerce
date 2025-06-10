using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderVM>> GetAllOrders();
        Task<OrderVM> GetOrderDetails(int id);
        Task<Order> CreateOrderAsync(int cartId);
        Task<CheckoutVM> CheckoutAsync(int orderId);
        Task<OrderVM> GetOrderVMAsync(Order order);
        Task<bool> ConfirmOrderAsync(int id);
    }
}
