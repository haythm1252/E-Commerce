using E_Commerce.Domain.Entities;

namespace E_Commerce.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
