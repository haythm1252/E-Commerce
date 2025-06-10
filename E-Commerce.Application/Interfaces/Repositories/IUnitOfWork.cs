

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get;}
        ICategoryRepository Categories { get; }
        IShoppingcartRepository ShoppingCarts { get; }
        IShoppingCartItemReporsitory ShoppingCartItems { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        Task<int> SaveChangesAsync();
    }
}
