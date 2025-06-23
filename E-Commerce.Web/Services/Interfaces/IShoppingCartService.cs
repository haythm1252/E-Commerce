using E_Commerce.Domain.Entities;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartWithProduct(int id);
        Task<bool> AddToCartAsync(int id);
        Task<bool> RemoveFromCartAsync(int id);
        Task<ShoppingCart> GetCart();
        Task<IEnumerable<ShoppingCartItem>> GetCartItems();
        Task RemoveAllCartItems(int cartId);
        Task AsignCartItemsToUser(List<ShoppingCartItem> items);
    }
}
