using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class MovieOpsController : Controller
    {
        private readonly IMovieRepo movieRepo;
        private readonly IActorRepo actorRepo;
        private readonly ICategoryrepo categoryRepo;
        private readonly IEcinemaRepo ecinemaRepo;

        public MovieOpsController(IMovieRepo movieRepo, IActorRepo actorRepo, ICategoryrepo categoryRepo, IEcinemaRepo ecinemaRepo)
        {
            this.movieRepo = movieRepo;
            this.actorRepo = actorRepo;
            this.categoryRepo = categoryRepo;
            this.ecinemaRepo = ecinemaRepo;
        }


        [HttpGet]
        public IActionResult Display()
        {
            var movies = movieRepo.Get(includes: [e => e.ActorMovies, e => e.Category, e => e.Cinema]);


            return View(movies);


        }








        [HttpGet]
        public IActionResult Create()
        {
            var categories = categoryRepo.Get();
            var cinemas = ecinemaRepo.Get();
            var actors = actorRepo.Get();
            MovieViewModel movieViewModel = new MovieViewModel
            {
                Categories = categories.ToList(),
                Cinemas = cinemas.ToList(),
                Actors = actors.ToList()
            };

            return View(movieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie();

                if (movieViewModel.Image != null && movieViewModel.Image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(movieViewModel.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/movies", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        movieViewModel.Image.CopyTo(stream);
                    }

                    movie.ImageUrl = fileName;
                }

                movie.Name = movieViewModel.Name;
                movie.Description = movieViewModel.Description;
                movie.Price = movieViewModel.Price;
                movie.StartDate = movieViewModel.StartDate;
                movie.EndDate = movieViewModel.EndDate;
                movie.CategoryId = movieViewModel.CategoryId;
                movie.CinemaId = movieViewModel.CinemaId;
                movie.TrailerUrl = movieViewModel.TrailerUrl;
                movie.ActorMovies = movieViewModel.SelectedActorIds?.Select(actorId => new ActorMovie
                {
                    ActorId = actorId
                }).ToList() ?? new List<ActorMovie>();

                movieRepo.Create(movie);
                movieRepo.Commit();

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var categories = categoryRepo.Get();
            var cinemas = ecinemaRepo.Get();
            var actors = actorRepo.Get();
            movieViewModel.Categories = categories.ToList();
            movieViewModel.Cinemas = cinemas.ToList();
            movieViewModel.Actors = actors.ToList();
            return View(movieViewModel);
        }

    }
}
