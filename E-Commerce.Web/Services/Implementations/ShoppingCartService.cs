using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.Services.Interfaces;
using System.Security.Claims;

namespace E_Commerce.Web.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContext;
        public ShoppingCartService(IUnitOfWork unitOfWork, IProductService productService, ICategoryService categoryService, IHttpContextAccessor httpcontext)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _httpContext = httpcontext;
        }
        
        public async Task<ShoppingCart> GetCart()
        {
            //cheak if user is authenticated (logged in) or not
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = await _unitOfWork.ShoppingCarts.FindAsync(c => c.CartId == userId);

                if (cart == null)
                {
                    //if no cart is found, create a new one
                    cart = new ShoppingCart() { CartId = userId };
                    await _unitOfWork.ShoppingCarts.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }

                return cart;
            }
            //if user is not authenticated, check if there is a CartId in the cookies
            var cartId = _httpContext.HttpContext.Request.Cookies["CartId"];
            if(cartId == null)
            {
                //if no CartId in the cookies, create a new one
                var newCartId = Guid.NewGuid().ToString();
                _httpContext.HttpContext.Response.Cookies.Append("CartId", newCartId, new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
                var cart = new ShoppingCart() { CartId = newCartId };
                await _unitOfWork.ShoppingCarts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
                return cart;
            }
            return await _unitOfWork.ShoppingCarts.FindAsync(c => c.CartId == cartId);
        }
        public async Task<bool> AddToCartAsync(int id)
        {
            var cart = await GetCart();
            var product = await _productService.GetByIdAsync(id);

            if (product == null || cart == null)
                return false;

            
            var cartItem = await _unitOfWork.ShoppingCartItems.FindAsync(c => c.ProductId == product.Id && c.ShoppingCartId == cart.Id);
            // if the cart item already exists, increment the quantity
            if (cartItem != null)
            {
                cartItem.Quantity++;
                _unitOfWork.ShoppingCartItems.Update(cartItem);
            }
            else
            {
                var CartItem = new ShoppingCartItem()
                {
                    ProductId = product.Id,
                    ShoppingCartId = cart.Id,
                    Quantity = 1,
                    UnitPrice = product.Price
                };
                await _unitOfWork.ShoppingCartItems.AddAsync(CartItem);
            }

            var res = await _unitOfWork.SaveChangesAsync();
            if (res > 0)
                return true;
            return false;
        }
        public async Task<bool> RemoveFromCartAsync(int id)
        {
            var cart = await GetCart();
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (cart == null || product == null)
                return false;

            var cartItem = await _unitOfWork.ShoppingCartItems.FindAsync(ci => ci.ProductId == id && ci.ShoppingCartId == cart.Id);
            if (cartItem !=null && cartItem.Quantity > 1 )
            {
                cartItem.Quantity--;
                _unitOfWork.ShoppingCartItems.Update(cartItem);
            }
            else
                _unitOfWork.ShoppingCartItems.Delete(cartItem);

            var res = await _unitOfWork.SaveChangesAsync();
            if (res > 0)
                return true;
            return false;
        }
        public async Task<IEnumerable<ShoppingCartItem>> GetCartItems()
        {
            var cart = await GetCart();
            if (cart == null)
                return null;
            var cartWithItems = await _unitOfWork.ShoppingCartItems.GetAllAsync(i => i.ShoppingCartId == cart.Id, i => i.Product);
            return cartWithItems;
        }

        public async Task<ShoppingCart> GetCartWithProduct(int id)
        {
            return await _unitOfWork.ShoppingCarts.GetCartWithProducts(id);
        }

        public async Task RemoveAllCartItems(int cartId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetByIdAsync(cartId);
            foreach(var item in cart.ShoppingCartItems)
            {
                _unitOfWork.ShoppingCartItems.Delete(item);
            }
            var res = await _unitOfWork.SaveChangesAsync();
            if(res==0)
                throw new ArgumentException("faild to delete cart items");
        }
    }
}
