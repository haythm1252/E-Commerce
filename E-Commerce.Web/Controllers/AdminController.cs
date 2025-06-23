using E_Commerce.Domain.Entities.Enums;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public AdminController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var todayOrders = _orderService.GetFilterdOrders(o => o.CreatedAt.Date == DateTime.Today && o.Status == OrderStatus.Success, pageNumber, pageSize);
            var sallesToday = (await _orderService.GetAllOrdersAsync(o => o.CreatedAt.Date == DateTime.Today && o.Status == OrderStatus.Success)).Sum(o => o.TotalPrice);

            var last30DaysOrders = _orderService.GetFilterdOrders(o => o.CreatedAt.Date >= DateTime.Today.AddDays(-30) && o.Status == OrderStatus.Success, pageNumber, pageSize);
            var sallesLast30 = (await _orderService.GetAllOrdersAsync(o => o.CreatedAt.Date >= DateTime.Today.AddDays(-30) && o.Status == OrderStatus.Success)).Sum(o => o.TotalPrice);

            var dashboardModel = new AdminDashboardVM 
            {
                TodayOrders = todayOrders,
                TotalSalesToday = sallesToday, 
                Last30DaysOrders = last30DaysOrders,
                TotalSalesLast30Days = sallesLast30
            };
            return View(dashboardModel);
        }
        public IActionResult TodayOrders(int pageNumber, int pageSize)
        {
            var todayOrders = _orderService.GetFilterdOrders(o => o.CreatedAt.Date == DateTime.Today && o.Status == OrderStatus.Success, pageNumber, pageSize);
            return PartialView("_TodayOrders",todayOrders);
        }
        public IActionResult Last30DayOrders(int pageNumber, int pageSize)
        {
            var last30DaysOrders = _orderService.GetFilterdOrders(o => o.CreatedAt.Date >= DateTime.Today.AddDays(-30) && o.Status == OrderStatus.Success, pageNumber, pageSize);
            return View("_Last30DaysOrders", last30DaysOrders);
        }
    }
}
