using Cinema.Identity;
using Cinema.Models;
using Cinema.Models.enums;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IcartRepo cartRepo;
        private readonly ICartItemsRepo cartItemRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMovieRepo movieRepo;

        public CartController(IcartRepo cartRepo ,ICartItemsRepo cartItemRepo,UserManager<ApplicationUser> userManager, IMovieRepo movieRepo)
        {
            this.cartRepo = cartRepo;
            this.cartItemRepo = cartItemRepo;
            this.userManager = userManager;
            this.movieRepo = movieRepo;
        }

        
        public IActionResult AddToCart(int movieId)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            // Ensure the movie exists
            var movieExists = movieRepo.Exists(movieId);  // Replace direct DB call with repo method
            if (!movieExists)
            {
                return BadRequest("Movie not found.");
            }

            // Get or create the cart
            var cart = cartRepo.GetCartByUserId(userId);
            var movie = movieRepo.GetOne(e=>e.Id==movieId);
            if(movie.MovieStatus == MovieStatus.Expired || movie.MovieStatus == MovieStatus.ComingSoon)
            {
                return BadRequest("Movie not available.");
            }

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    CartItems = new List<CartItems>()
                };

                cartRepo.Create(cart);
                cartRepo.Commit();
            }

            // Check if the movie is already in the cart
            var existingCartItem = cartItemRepo.GetCartItem(cart.Id, movieId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                cartItemRepo.Edit(existingCartItem);
            }
            else
            {
                var cartItem = new CartItems
                {
                    CartId = cart.Id,
                    MovieId = movieId,
                    Quantity = 1
                };

                cartItemRepo.Create(cartItem);
            }

            cartItemRepo.Commit();
            TempData["SuccessMessage"] = "Movie added to cart successfully!";
            return RedirectToAction("Index", "Movie", new { area = "Customer" });
        }


        [HttpGet]
        public IActionResult DisplayCart()
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null) return RedirectToAction("Index", "Movie", new { area = "Customer" });
           
            return View("DisplayCart", cart);
        }

    }
}
