using E_Commerce.Application.Common;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.GetPagedAsync(pageNumber, pageSize, null, p => p.Category);
            return View(products);
        }
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ProductTable(int pageNumber, int pageSize)
        {
            var products = await _productService.GetPagedAsync(pageNumber, pageSize, null, p => p.Category);
            return PartialView("_ProductTable", products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.Details(id);

            if (product == null)
                return View("NotFound");

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductVM { Categories = await _categoryService.GetSelectList() };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
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
        [Authorize(Roles = Roles.Admin)]
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
                BestSeller = product.BestSeller,
                Categories = await _categoryService.GetSelectList()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _productService.DeleteAsync(id) ? Ok() : BadRequest();
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> SearchById(int id)
        {
            var model = await _productService.SearchById(id);
            return PartialView("_ProductTable", model);
        }

        public async Task<IActionResult> Search(string query, int pageNumber = 1, int pageSize = 12)
        {
            var products = await _productService.GetPagedAsync(
                pageNumber,
                pageSize,
                p =>  p.Name.Contains(query) || p.Description.Contains(query),
                p => p.Category
            );
            var categories = await _categoryService.GetSelectList();

            var model = new SearchVM
            {
                Products = products,
                Categories = categories,
                query = query
            };
            return View(model);
        }
    }
}
