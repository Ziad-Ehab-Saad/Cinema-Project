using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Cinema.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IActorMovieRepo actorMovieRepo;

        public MovieOpsController(IMovieRepo movieRepo, IActorRepo actorRepo, ICategoryrepo categoryRepo, IEcinemaRepo ecinemaRepo,IActorMovieRepo actorMovieRepo)
        {
            this.movieRepo = movieRepo;
            this.actorRepo = actorRepo;
            this.categoryRepo = categoryRepo;
            this.ecinemaRepo = ecinemaRepo;
            this.actorMovieRepo = actorMovieRepo;
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

        public IActionResult Delete(int id)
        {
            var movie = movieRepo.GetOne(e=>e.Id==id);
            if (movie == null)
            {
                return NotFound();
            }
            movieRepo.Delete(movie);
            movieRepo.Commit();
            return RedirectToAction("Display", "MovieOps", new { area = "Admin" });
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = movieRepo.GetOne(e => e.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var categories = categoryRepo.Get();
            var cinemas = ecinemaRepo.Get();
            var actors = actorRepo.Get();
            var movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Categories = categories.ToList(),
                Cinemas = cinemas.ToList(),
                Actors = actors.ToList(),
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,
                TrailerUrl = movie.TrailerUrl,
                SelectedActorIds = movie.ActorMovies.Select(e => e.ActorId).ToList(),
                ImageUrl=movie.ImageUrl


            };
            return View(movieViewModel);



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieViewModel movieViewModel, int id)
        {
            if (!ModelState.IsValid)
            {
                movieViewModel.Actors = actorRepo.Get().ToList();
                movieViewModel.Categories = categoryRepo.Get().ToList();
                movieViewModel.Cinemas = ecinemaRepo.Get().ToList();
                return View(movieViewModel);
            }

            var movie = movieRepo.GetOne(m => m.Id == id);
            if (movie == null) return NotFound();

            movie.Name = movieViewModel.Name;
            movie.Description = movieViewModel.Description;
            movie.Price = movieViewModel.Price;
            movie.StartDate = movieViewModel.StartDate;
            movie.EndDate = movieViewModel.EndDate;
            movie.TrailerUrl = movieViewModel.TrailerUrl;
            movie.CategoryId = movieViewModel.CategoryId;
            movie.CinemaId = movieViewModel.CinemaId;

            if (movieViewModel.Image != null && movieViewModel.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/movies");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + movieViewModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    movieViewModel.Image.CopyTo(fileStream);
                }

                if (!string.IsNullOrEmpty(movie.ImageUrl))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, movie.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                movie.ImageUrl = uniqueFileName;
            }

            if (movieViewModel.SelectedActorIds != null && movieViewModel.SelectedActorIds.Any())
            {
                var existingActorMovies = actorMovieRepo.Get(am => am.MovieId == id).ToList();

                foreach (var actorMovie in existingActorMovies)
                {
                    actorMovieRepo.Delete(actorMovie);
                }

                foreach (var actorId in movieViewModel.SelectedActorIds)
                {
                    actorMovieRepo.Create(new ActorMovie
                    {
                        MovieId = id,
                        ActorId = actorId
                    });
                }
            }

            movieRepo.Update(movie); 

            return RedirectToAction("Display", "MovieOps");
        }




    }
}
