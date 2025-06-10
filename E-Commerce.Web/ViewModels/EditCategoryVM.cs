using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Web.ViewModels
{
    public class EditCategoryVM
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } =default!;

        [DisplayName("Add to Top Category Section")]
        public bool IsTopCategory { get; set; } = false;

        [DisplayName("Add Section For this Category")]
        public bool IsSection { get; set; } = false;

        [ValidateNever]
        public string? CurrentImage { get; set; }

        public int Id { get; set; } 
    }
}
