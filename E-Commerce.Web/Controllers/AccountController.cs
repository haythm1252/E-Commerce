using E_Commerce.Application.Common;
using E_Commerce.Infrastructure.Identity;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int pageNumber = 1,int pageSize = 10)
        {
            //that is very bad way for performance because i am loading all the users at onece i will handel it in another time
            var AllUsers = await _userManager.GetUsersInRoleAsync("Customer");
            var users = AllUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var model = new PagedResult<ApplicationUser> { Items = users, TotalCount = AllUsers.Count(), PageNumber = pageNumber, PageSize = pageSize };
            return View(model);
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
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

        [HttpGet]
        public async Task<IActionResult> Edit ()
        {
            //var user = await _userManager.FindByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            return View(user);
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> SaveEdit(ApplicationUser model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var user = await _userManager.GetUserAsync(User);

            user.UserName = model.Email;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View("Edit", model);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View("ResetPassword", model);
            if (model.NewPassword == model.OldPassword)
            {
                ModelState.AddModelError("NewPassword", "The new password must be different from the old password.");
                return View("ResetPassword", model);
            }
            var user = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View("ResetPassword", model);
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }
    }
}
