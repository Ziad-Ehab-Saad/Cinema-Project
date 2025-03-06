using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieRepo movieRepo;
        private readonly IActorRepo actorRepo;
        public MovieController(IMovieRepo movieRepo, IActorRepo actorRepo)
        {
            this.movieRepo = movieRepo;
            this.actorRepo = actorRepo;
        }

        public IActionResult Index()
        {
            ViewBag.RegisterMessage = TempData["SucessRegister"];
            ViewBag.LoginMessage = TempData["SucessUserLogin"];
            
            ViewBag.SucessLogin = TempData["SucessLogin"];
            var movies = movieRepo.Get(includes: [e => e.ActorMovies, e => e.Category, e => e.Cinema]);


            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = movieRepo.GetMovieWithDetails(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel
            {
                movie = movie,
                Actors = movie.ActorMovies?.Select(e => e.Actor).ToList() ?? new List<Actor>()
            };

            return View(movieDetailsViewModel);
        }

        public IActionResult Search(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var movies = movieRepo.Get(m => m.Name.Contains(name), includes: [e => e.ActorMovies, e => e.Category, e => e.Cinema]);
            return View("Index", movies.ToList());
        }




    }
}
