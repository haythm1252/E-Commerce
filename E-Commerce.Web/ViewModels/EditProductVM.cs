using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Web.ViewModels
{
    public class EditProductVM
    {
        public int Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1500)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; } = default!;

        [Display(Name = "Add to Best Seller section")]
        public bool BestSeller { get; set; } = false;

        [Required]
        public int Stock { get; set; }

        [ValidateNever]
        public string? CurrentImage { get; set; } 

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
