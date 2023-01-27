using Freelancer.Models;
using Freelancer.Utilities.Enum;
using Freelancer.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Freelancer.Controllers
{
    public class AccountController : Controller
    {
       UserManager<AppUser> _userManager;
    SignInManager<AppUser> _signInManager;  
       RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            AppUser user = await _userManager.FindByNameAsync(registerVM.Username);
            if (user != null)
            {
                ModelState.AddModelError("", "bu adda user var");
                return View();
            }
            user = new AppUser
            {
                UserName= registerVM.Username,
                Email=registerVM.Email,
                FirstName=registerVM.Name, 
                LastName=registerVM.Name
            };
            var result = await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                };
                return View();
            }
            await _userManager.AddToRoleAsync(user,Roles.Admin.ToString());
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string? returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user == null) 
            {
                await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError("", "name or pass wrong");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistance, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "login ve ya pass wrong");
                return View();
            }
            await _signInManager.SignInAsync(user, false); 
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> AddRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
            return View();
        }

    }
}
