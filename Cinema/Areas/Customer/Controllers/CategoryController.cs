using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {
        private readonly ICategoryrepo categoryrepo;

        public CategoryController(ICategoryrepo categoryrepo )
        {
            this.categoryrepo = categoryrepo;
        }

        public IActionResult Index()
        {

            var categories = categoryrepo.Get().ToList();
            return View(categories);
        }
        public IActionResult Details(int id)
        {
            var category = categoryrepo.GetCategoryWithDetails(e=> e.Id == id);
            return View(category);
        }


    }
}
