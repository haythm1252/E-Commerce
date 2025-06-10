using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Identity;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System.Security.Claims;

namespace E_Commerce.Web.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IPaymobService _paymobServices;
        private readonly IShoppingCartService _shoppingCartServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderService(IPaymobService paymob,IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager,
            IShoppingCartService shoppingCartServices)
        {
            _paymobServices = paymob;
            _shoppingCartServices = shoppingCartServices;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Order> CreateOrderAsync(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCarts.FindAsync(c => c.Id == cartId, c => c.ShoppingCartItems);
            if (cart == null)
                throw new ArgumentException("Cart not found");

            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new InvalidOperationException("User is not authenticated");

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User ID is not available");

            var order = new Order()
            {
                CreatedAt = DateTime.Now,
                TotalPrice = cart.TotalPrice ,
                OrderItems = cart.ShoppingCartItems.Select(items => new OrderItem()
                {
                    ProductId = items.ProductId,
                    Quantity = items.Quantity,
                    UnitPrice = items.UnitPrice,
                }).ToList(),
                UserId = userId
            };
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var orderWithProducts = await _unitOfWork.Orders.GetOrderWithProducts(order.Id); 
            return orderWithProducts;
        }
        public async Task<CheckoutVM> CheckoutAsync(int cartId)
        {
            var order = await CreateOrderAsync(cartId);
            var orderVM = await GetOrderVMAsync(order);
            var authToken = await _paymobServices.GetAuthTokenAsync();
            var orderId = await _paymobServices.CreateOrderAsync(authToken, orderVM);
            var paymentToken = await _paymobServices.GetPaymentTokenAsync(authToken, orderId, orderVM);

            order = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            order.PaymentId = orderId;
            var res = await _unitOfWork.SaveChangesAsync();
            if(res == 0)
                throw new InvalidOperationException("order did not creat");

            var checkoutVM = new CheckoutVM
            {
                PaymentToken = paymentToken,
                IframeId = "928886"
            };
            await _shoppingCartServices.RemoveAllCartItems(cartId);
            return checkoutVM;
        }
        public async Task<OrderVM> GetOrderVMAsync(Order order)
        {
            var user = await _userManager.FindByIdAsync(order.UserId);
            if (user == null)
                throw new ArgumentException("User not found");
            
            var orderVM = new OrderVM
            {
                Order = order,
                User = user
            };
            return orderVM;
        }

        public Task<List<OrderVM>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<OrderVM> GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            var authToken = await _paymobServices.GetAuthTokenAsync();
            var paymentStatus = false;

         
                paymentStatus = await _paymobServices.IsPaymentSuccessfully(order.PaymentId, authToken);

            order.IsPaid = paymentStatus;
            await _unitOfWork.SaveChangesAsync();
            return paymentStatus;
        }
    }
}
