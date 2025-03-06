using Cinema.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleAdminController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {

                IdentityRole role = new IdentityRole
                {
                    Name = roleViewModel.Name
                };

                IdentityResult identityResult = await roleManager.CreateAsync(role);
                if (identityResult.Succeeded)
                {
                    TempData["Success"] = "Role Created Successfully";

                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                }

                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", "Role is already exists");
                }
            }
            return View(roleViewModel);
        }

    }
}
