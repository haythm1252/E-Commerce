using E_Commerce.Domain.Entities.Enums;
using E_Commerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetFilterdOrders(OrdersFilters.All.ToString());
            return View(orders);
        }
        public async Task<IActionResult> OrderTable(string filter)
        {
            var orders = await _orderService.GetFilterdOrders(filter);
            return PartialView("_OrderTable",orders);
        }
        public async Task<IActionResult> Search (int id)
        {
            var orderVM = await _orderService.SearchById(id);
            return PartialView("_OrderTable", orderVM);
        }
        public async Task<IActionResult> Checkout(int cartId)
        {
            var model = await _orderService.CheckoutAsync(cartId);
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var orderVM = await _orderService.GetOrderDetails(id);
            if (orderVM == null)
                return NotFound();
            return View(orderVM);
        }
        // i did this action to verify the payment transaction since this is porject on lockal server so i couldnt just add callback url in paymob so i just made this 
        // instand just consedring all orders are confirmed but this way still need manual acting it need someone click a link like /Order/OrderConfirm/id
        public async Task<IActionResult> ConfirmOrders()
        {
            var (success, faild) = await _orderService.ConfirmAllOrdersAsync();
            TempData["ConfirmOrders"] = success == 0 && faild == 0 ? "All orders are already confirmed." :
                $"{success} orders confirmed successfully, {faild} orders failed to confirm.";
            return RedirectToAction("Index");
        }
    }
}
