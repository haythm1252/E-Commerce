using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;
        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var category= await _CategoryService.GetAllAsync(null,c => c.Products);
            return View(category);
        }
    }
}
