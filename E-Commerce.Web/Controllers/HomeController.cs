using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Web.Models;
using E_Commerce.Web.Services.Interfaces;
using System.Threading.Tasks;
using E_Commerce.Web.ViewModels;

namespace E_Commerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public HomeController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var homeVM = new HomeVM()
        {
            Categories = await _categoryService.GetAllAsync(c => c.IsTopCategory == true || c.IsSection == true, c => c.Products),
            Products = await _productService.GetAllAsync(p => p.BestSeller == true, p => p.Category)
        };
        return View(homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
