using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Identity;

namespace E_Commerce.Web.ViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public ApplicationUser User { get; set; }
    }
}
