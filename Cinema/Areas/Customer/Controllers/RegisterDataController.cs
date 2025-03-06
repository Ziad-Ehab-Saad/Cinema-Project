using Cinema.Identity;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cinema.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class RegisterDataController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public RegisterDataController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserData registerUserData)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerUserData.Name,
                    Email = registerUserData.Email,
                    Address = registerUserData.Address
                };

                if (user != null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, registerUserData.Password);
                    if (result.Succeeded)
                    {

                        await signInManager.SignInAsync(user, isPersistent: false);
                        TempData["SucessRegister"] = "User Register Sucessfully";
                      return  RedirectToAction("Index", "Movie", new { area = "Customer" });

                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }



                }

            }

            return View(registerUserData);





        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserData loginUserData)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByNameAsync(loginUserData.Name);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginUserData.Password, loginUserData.RememberMe,false);

                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("admin"))
                        {
                            TempData["SucessAdminLogin"] = "Admin Login Sucessfully";
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }

                        else
                        {
                            TempData["SucessUserLogin"] = "User Login Sucessfully";
                            return RedirectToAction("Index", "Movie", new { area = "Customer" });
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(loginUserData);
        }

    }
}
