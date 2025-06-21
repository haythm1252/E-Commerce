using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Web.ViewModels
{
    public class SearchVM
    {
        public PagedResult<Product> Products { get; set; } = new PagedResult<Product>();
        public string query { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
