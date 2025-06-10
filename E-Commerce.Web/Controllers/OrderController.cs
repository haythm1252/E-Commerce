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
            return View();
        }
        public async Task<IActionResult> Checkout(int cartId)
        {
            var model = await _orderService.CheckoutAsync(cartId);
            return View(model);
        }
        public async Task<IActionResult> OrderDetails(int id)
        {
            var orderDetails = await _orderService.GetOrderDetails(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }
        // i did this action to verify the payment transaction since this is porject on lockal server so i couldnt just add callback url in paymob so i just made this 
        // instand just consedring all orders are confirmed but this way still need manual acting it need someone click a link like /Order/OrderConfirm/id
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var status = await _orderService.ConfirmOrderAsync(id);
            return status ? Ok() : NotFound();
        }
    }
}
