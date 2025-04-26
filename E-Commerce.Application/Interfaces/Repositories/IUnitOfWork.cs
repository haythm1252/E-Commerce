

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get;}
        ICategoryRepository Categories { get; }
        int Save();
    }
}
