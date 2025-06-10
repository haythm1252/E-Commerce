using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.Web.ViewModels
{
    public class CreateCategoryVM
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public IFormFile Image { get; set; } = default!;

        [DisplayName("Add to Top Category Section")]
        public bool IsTopCategory { get; set; } = false;

        [DisplayName("Add Section For this Category")]
        public bool IsSection { get; set; } = false;
    }
}
