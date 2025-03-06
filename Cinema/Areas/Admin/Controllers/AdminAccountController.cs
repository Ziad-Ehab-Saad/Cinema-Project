using Cinema.Identity;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AdminAccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public RoleManager<IdentityRole> RoleManager { get; }

        [HttpGet]
        public async Task< IActionResult> Register()
        {
            var roles = await roleManager.Roles.ToListAsync(); 
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AdminViewModel adminViewModel)
        {
            var roles = await roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser
                {
                    UserName = adminViewModel.Name,
                    Email = adminViewModel.Email,
                    Address = adminViewModel.Address,

                };
                if (user != null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, adminViewModel.Password);
                    if (result.Succeeded)
                    {
                     //await signInManager.SignInAsync(user, isPersistent: false);

                        await userManager.AddToRoleAsync(user, adminViewModel.SelectedRole);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);


                    }
                }
            }
            return View(adminViewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

    }
}
