using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Enums;
using E_Commerce.Infrastructure.Identity;
using E_Commerce.Infrastructure.Migrations;
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

        public List<OrderVM> GetOrderVMsAsync(List<Order> orders)
        {
            var userIds = orders.Select(o => o.UserId).Distinct().ToList();
            var users =  _userManager.Users.Where(u => userIds.Contains(u.Id)).ToList();
            var orderVMs = orders.Select(order =>
            {
                var user = users.FirstOrDefault(u => u.Id == order.UserId);
                if (user == null)
                    throw new ArgumentException($"User with ID {order.UserId} not found");

                return new OrderVM
                {
                    Order = order,
                    User = user
                };
            }).ToList();
            return orderVMs;
        }

        public async Task<List<OrderVM>> GetFilterdOrders(string filter)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var filteredOrders = orders.Where(o => o.Status.ToString() == filter || filter == OrdersFilters.All.ToString()).ToList(); // fillter = all or pending or success or faild

            if(filter == OrdersFilters.Today.ToString()) 
                filteredOrders = orders.Where(o => o.CreatedAt.Date == DateTime.Today).ToList();
            if(filter == OrdersFilters.Last30Days.ToString())
                filteredOrders = orders.Where(o => o.CreatedAt >= DateTime.Today.AddDays(-30)).ToList();

            var orderVMs = GetOrderVMsAsync(filteredOrders);
            return orderVMs;
        }

        public async Task<OrderVM> GetOrderDetails(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderWithProducts(id);
            if (order == null)
                throw new ArgumentException("Order not found");
            return await GetOrderVMAsync(order);
        }
        public async Task<bool> ConfirmOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            var authToken = await _paymobServices.GetAuthTokenAsync();
            var paymentStatus = await _paymobServices.IsPaymentSuccessfully(order.PaymentId, authToken);

            if (paymentStatus)
            {
                order.Status = OrderStatus.Success;
                await _unitOfWork.SaveChangesAsync();
                return paymentStatus;
            }
            order.Status = OrderStatus.Faild;
            await _unitOfWork.SaveChangesAsync();
            return paymentStatus;

        }
        public async Task<(int success, int faild)> ConfirmAllOrdersAsync()
        {
            var pendingOrders = await _unitOfWork.Orders.GetAllOrderWithProducts(o => o.Status == OrderStatus.Pending);
            if (!pendingOrders.Any())
                return (0, 0); 
            var authToken = await _paymobServices.GetAuthTokenAsync();

            // here i made task.when all and used select rathere than foreach to make it better 
            //task.whenall will do all tasks in the same time not one by one
            var results = await Task.WhenAll(pendingOrders.Select(async order =>
            {
                bool isSuccess = await _paymobServices.IsPaymentSuccessfully(order.PaymentId, authToken);
                if (isSuccess)
                {
                    foreach (var item in order.OrderItems)
                    {
                        item.Product.Stock -= item.Quantity;
                    }
                }
                order.Status = isSuccess ? OrderStatus.Success : OrderStatus.Faild;
                return isSuccess;
            }));

            int successCount = results.Count(r => r);
            int faildCount = results.Count(r => !r);

            await _unitOfWork.SaveChangesAsync(); 

            return (successCount, faildCount);
        }

        // i made it return list cuz i dont want to make another partial view that has model only one order vm so i used the same partial view that 
        // display the list of ordervm
        public async Task<List<OrderVM>> SearchById(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return new List<OrderVM>();
            var vm = await GetOrderVMAsync(order);

            return new List<OrderVM> { vm };
        }

        public async Task<UserDetailsVM> GetUserWithOrders(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.UserId == userId);
            if (orders == null)
                return new UserDetailsVM { User = user };

            var orderVMs = orders.Select(order => new OrderVM { User = user, Order = order }).ToList();
            return new UserDetailsVM { User = user, Orders = orderVMs };
        }
    }
}
