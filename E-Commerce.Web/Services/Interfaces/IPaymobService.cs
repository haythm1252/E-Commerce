using E_Commerce.Web.ViewModels;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IPaymobService
    {
        Task<string> GetAuthTokenAsync();
        Task<string> CreateOrderAsync(string authToken, OrderVM order);
        Task<string> GetPaymentTokenAsync(string authToken, string orderId, OrderVM order);
        Task<bool> IsPaymentSuccessfully(string orderId, string authToken);
    }
}
