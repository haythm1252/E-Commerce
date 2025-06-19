using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync(null, p => p.Category);
            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.Details(id);

            if (product == null) 
                return View("NotFound");

            return View(product); 
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductVM { Categories = await _categoryService.GetSelectList() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetSelectList();
                return View(model);
            }

            TempData["Message"] = await _productService.AddAsync(model) ? "Product Created Successfully" : "Product Creation Failed";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return View("NotFound");
            var model = new EditProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Stock = product.Stock,
                CurrentImage = product.ImageUrl,
                Categories = await _categoryService.GetSelectList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetSelectList();
                return View(model);
            }
            TempData["Message"] = await _productService.EditAsync(model) ? "Product Updated Successfully" : "Product Update Failed";
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await _productService.DeleteAsync(id) ? Ok() : BadRequest();
        }
        public async Task<IActionResult> SearchById(int id)
        {
            var model = await _productService.SearchById(id);
            return PartialView("_ProductTable", model);
        }
    }
}
