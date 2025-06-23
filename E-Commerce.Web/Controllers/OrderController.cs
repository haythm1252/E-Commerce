using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Enums;
using E_Commerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index(int pageNumber = 1,int pageSize = 10)
        {
            var orders =  _orderService.GetFilterdOrders(OrdersFilters.All.ToString(), pageNumber,  pageSize);
            return View(orders);
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult OrderTable(string filter, int pageNumber = 1, int pageSize = 10)
        {
            var orders = _orderService.GetFilterdOrders(filter, pageNumber, pageSize);
            return PartialView("_OrderTable",orders);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Search (int id)
        {
            var orderVM = await _orderService.SearchById(id);
            return PartialView("_OrderTable", orderVM);
        }

        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> Checkout(int cartId)
        {
            var model = await _orderService.CheckoutAsync(cartId);
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var orderVM = await _orderService.GetOrderDetails(id);
            if (orderVM == null)
                return NotFound();
            return View(orderVM);
        }
        // i did this action to verify the payment transaction since this is porject on lockal server so i couldnt just add callback url in paymob so i just made this 
        // instand just consedring all orders are confirmed but this way still need manual acting it need someone click a link like /Order/OrderConfirm/id
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ConfirmOrders()
        {
            var (success, faild) = await _orderService.ConfirmAllOrdersAsync();
            TempData["ConfirmOrders"] = success == 0 && faild == 0 ? "All orders are already confirmed." :
                $"{success} orders confirmed successfully, {faild} orders failed to confirm.";
            return RedirectToAction("Index");
        }
    }
}
