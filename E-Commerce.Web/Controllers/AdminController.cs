using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
