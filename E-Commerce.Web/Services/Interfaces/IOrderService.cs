using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderVM>> GetFilterdOrders(string filter);
        // i made it return list cuz i dont want to make another partial view that has model only one order vm so i used the same partial view that 
        // display the list of ordervm
        Task<List<OrderVM>> SearchById(int id);
        Task<UserDetailsVM> GetUserWithOrders(string userId);
        Task<OrderVM> GetOrderDetails(int id);
        Task<Order> CreateOrderAsync(int cartId);
        Task<CheckoutVM> CheckoutAsync(int orderId);
        Task<OrderVM> GetOrderVMAsync(Order order);
        Task<bool> ConfirmOrderAsync(int id);
        Task<(int success, int faild)> ConfirmAllOrdersAsync();
    }
}
