using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ActorOpsController : Controller
    {
        private readonly IActorRepo actorRepo;

        public ActorOpsController(IActorRepo actorRepo)
        {
            this.actorRepo = actorRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var actors = actorRepo.Get().ToList();

            return View(actors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActorOpsViewModel actorOpsViewModel)
        {
            if (ModelState.IsValid)
            {
                var Actor = new Actor()
                {
                    FirstName = actorOpsViewModel.FirstName,
                    LastName = actorOpsViewModel.LastName,
                    Bio = actorOpsViewModel.Bio,
                    News = actorOpsViewModel.News
                };

                if (actorOpsViewModel.Image != null && actorOpsViewModel.Image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(actorOpsViewModel.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/cast", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        actorOpsViewModel.Image.CopyTo(stream);
                    }

                    Actor.ProfilePicture = fileName; 
                }

                
                actorRepo.Create(Actor);
                actorRepo.Commit();

                return RedirectToAction("Index");
            }

            return View(actorOpsViewModel);
        }

        
        public IActionResult Delete(int id)
        {
            var actor = actorRepo.GetOne(e=>e.Id==id);
            if (actor == null)
            {
                return NotFound();
            }
            actorRepo.Delete(actor);
            actorRepo.Commit();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var actor = actorRepo.GetOne(e => e.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            var actorOpsViewModel = new ActorOpsViewModel()
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                Bio = actor.Bio,
                News = actor.News,
                ProfilePicture = actor.ProfilePicture
            };
            return View(actorOpsViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ActorOpsViewModel actorOpsViewModel, int id)
        {
            var actor = actorRepo.GetOne(e => e.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                actor.FirstName = actorOpsViewModel.FirstName;
                actor.LastName = actorOpsViewModel.LastName;
                actor.Bio = actorOpsViewModel.Bio;
                actor.News = actorOpsViewModel.News;

                if (actorOpsViewModel.Image != null && actorOpsViewModel.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/cast");

                  
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + actorOpsViewModel.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                   
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        actorOpsViewModel.Image.CopyTo(fileStream);
                    }

                    
                    if (!string.IsNullOrEmpty(actor.ProfilePicture))
                    {
                        string oldImagePath = Path.Combine(uploadsFolder, actor.ProfilePicture);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                  
                    actor.ProfilePicture = uniqueFileName;
                }

               
                actorRepo.Update(actor);
                actorRepo.Commit();

                return RedirectToAction("Index");
            }

            return View(actorOpsViewModel);
        }



    }
}
