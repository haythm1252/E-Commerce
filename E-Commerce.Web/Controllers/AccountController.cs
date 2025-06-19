using E_Commerce.Infrastructure.Identity;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrderService _orderService;
        public AccountController(UserManager<ApplicationUser> UserManager,SignInManager<ApplicationUser> signInManager, 
                RoleManager<IdentityRole> roleManager, IOrderService orderService)
        {
            _userManager = UserManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            return View(users);
        }
        public async Task<IActionResult> Profile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userDetailsVM = await _orderService.GetUserWithOrders(user!.Id);
            if (userDetailsVM == null)
                return NotFound();

            return View(userDetailsVM);
        }
        public async Task<IActionResult> Details(string id)
        {
            var userDetailsVM = await _orderService.GetUserWithOrders(id);
            if (userDetailsVM == null)
                return NotFound();

            return View(userDetailsVM);
        }
        public async Task<IActionResult> SearchById(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("User Email cannot be null or empty.");
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound($"User with Email {Email} not found.");
            }
            return PartialView("_UsersTable", new List<ApplicationUser> { user });
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> SaveRegister(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // check if the user is already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedAt = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            // add user to role
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> SaveLogin(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // loging in the user
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", "Invalid login attempt");
                return View(model);
            }

            // check the user in which role
            var user = await _userManager.FindByEmailAsync(model.Email);
            var UserRole = await _userManager.GetRolesAsync(user!);
            // check if the user is in admin role or customer role
            if (user != null && UserRole.Contains("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user != null && UserRole.Contains("Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
