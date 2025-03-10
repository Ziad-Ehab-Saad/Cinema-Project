using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryrepo categoryrepo;

        public CategoryController(ICategoryrepo categoryrepo)
        {
            this.categoryrepo = categoryrepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = categoryrepo.Get();
            return View(categories.ToList());

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                categoryrepo.Create(category);
                categoryrepo.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            var category = categoryrepo.GetOne(e => e.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            categoryrepo.Delete(category);
            categoryrepo.Commit();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = categoryrepo.GetOne(e => e.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category, int id)
        {
            ModelState.Remove("Movies");

            if (ModelState.IsValid)
            {
                categoryrepo.Update(category);
                categoryrepo.Commit();
                return RedirectToAction("Index");
            }

            return View(category);
        }




    }
}
