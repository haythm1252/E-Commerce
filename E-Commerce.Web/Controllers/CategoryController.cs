using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.Helpers;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
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
            var category = await _CategoryService.GetAllAsync(null, c => c.Products);
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _CategoryService.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _CategoryService.GetByIdAsync(id)!;
            if (category == null)
                return View("NotFound");
            var model = new EditCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                IsTopCategory = category.IsTopCategory,
                IsSection = category.IsSection,
                CurrentImage = category.ImageUrl
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _CategoryService.EditAsync(model);

            TempData["Message"] = result ? "Category updated successfully." : "Error occurred while updating the category.";

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _CategoryService.DeleteAsync(id);
            return result ? Ok() : BadRequest();
        }

        public async Task<IActionResult> Products(int id, int pageNumber = 1, int pageSize = 12)
        {
            var pagedResult = await _CategoryService.GetProducts(id, pageNumber, pageSize);
            var model = new SearchVM
            {
                Products = pagedResult,
                Categories = await _CategoryService.GetSelectList(),
            };
            return View(model);
        }
    }
}
