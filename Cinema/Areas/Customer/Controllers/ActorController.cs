using Cinema.Repositories.IRepositories;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ActorController : Controller
    {
        private readonly IActorRepo actorRepo;
        private readonly IMovieRepo movieRepo;
        public ActorController(IActorRepo actorRepo)
        {

            this.actorRepo = actorRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            //var actor = actorRepo.GetOne(e => e.Id == id, includes: [mov =>mov.]);
            //return View(actor);

            var actor = actorRepo.GetActorWithDetails(e => e.Id == id);
            ActorsDetails actorsDetails = new ActorsDetails
            {
                actor = actor,
                movies = actor.ActorMovies.Select(e => e.Movie).ToList()
            };


            return View(actorsDetails);
        }
    }
}
