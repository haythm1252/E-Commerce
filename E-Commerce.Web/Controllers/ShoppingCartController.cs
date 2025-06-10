using E_Commerce.Domain.Entities;
using E_Commerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
            var cartItems = await _shoppingCartService.GetCartItems();
            return View(cartItems);
        }
        public async Task<IActionResult> AddToCart(int Id)
        {
            return await _shoppingCartService.AddToCartAsync(Id) ? Ok() : BadRequest("Failed to add item to cart.");
        }
        public async Task<IActionResult> RemoveFromCart(int Id)
        {
            return await _shoppingCartService.RemoveFromCartAsync(Id) ? Ok() : BadRequest("Failed to add item to cart.");
        }
        public async Task<IActionResult> Checkout (int cartId)
        {
            var cart = await _shoppingCartService.GetCartWithProduct(cartId);
            return View(cart);
        }
    }
}
