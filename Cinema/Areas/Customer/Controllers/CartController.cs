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

        public CartController(IcartRepo cartRepo, ICartItemsRepo cartItemRepo, UserManager<ApplicationUser> userManager, IMovieRepo movieRepo)
        {
            this.cartRepo = cartRepo;
            this.cartItemRepo = cartItemRepo;
            this.userManager = userManager;
            this.movieRepo = movieRepo;
        }


        public IActionResult AddToCart(int movieId, int Count)
        {
            if (Count <= 0)
            {
                return BadRequest("Invalid quantity.");
            }

            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            var movieExists = movieRepo.Exists(movieId);
            if (!movieExists)
            {
                return BadRequest("Movie not found.");
            }

            var movie = movieRepo.GetOne(e => e.Id == movieId);
            //if (movie.MovieStatus == MovieStatus.Expired || movie.MovieStatus == MovieStatus.ComingSoon)
            //{
            //    return BadRequest("Movie not available.");
            //}

            var cart = cartRepo.GetCartByUserId(userId);
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

            var existingCartItem = cartItemRepo.GetCartItem(cart.Id, movieId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += Count; 
                cartItemRepo.Edit(existingCartItem);
            }
            else
            {
                var cartItem = new CartItems
                {
                    CartId = cart.Id,
                    MovieId = movieId,
                    Quantity = Count
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

        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            var cartItem = cartItemRepo.GetOne(e => e.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            // Ensure the item belongs to the user's cart
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null || cartItem.CartId != cart.Id)
            {
                return BadRequest("Unauthorized action.");
            }

            cartItemRepo.Delete(cartItem);
            cartItemRepo.Commit();

            TempData["SuccessMessage"] = "Movie removed from cart successfully!";
            return RedirectToAction(nameof(DisplayCart));
        }

        public IActionResult Increment(int cartItemId)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }
            var cartItem = cartItemRepo.GetOne(e => e.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null || cartItem.CartId != cart.Id)
            {
                return BadRequest("Unauthorized action.");
            }
            cartItem.Quantity++;
            cartItemRepo.Edit(cartItem);
            cartItemRepo.Commit();
            TempData["SuccessMessage"] = "Quantity updated successfully!";
            return RedirectToAction(nameof(DisplayCart));



        }
        public IActionResult Decrement(int cartItemId)
        {

            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }
            var cartItem = cartItemRepo.GetOne(e => e.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null || cartItem.CartId != cart.Id)
            {
                return BadRequest("Unauthorized action.");
            }
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;

                cartItemRepo.Edit(cartItem);
                cartItemRepo.Commit();

                TempData["SuccessMessage"] = "Quantity updated successfully!";
            }
            return RedirectToAction(nameof(DisplayCart));



        }

    }

}
