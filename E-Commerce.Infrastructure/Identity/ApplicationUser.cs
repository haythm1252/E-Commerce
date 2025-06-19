
namespace E_Commerce.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? CreatedAt{ get; set; } 
        // relationships
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
