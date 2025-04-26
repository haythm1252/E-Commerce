using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Web.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;

        [Required]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number is not valid")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        public string Address { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and be at least 8 characters long.")]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
