using E_Commerce.Infrastructure.Identity;

namespace E_Commerce.Web.ViewModels
{
    public class UserDetailsVM
    {
        public ApplicationUser User { get; set; }
        public List<OrderVM>? Orders { get; set; }
    }
}
