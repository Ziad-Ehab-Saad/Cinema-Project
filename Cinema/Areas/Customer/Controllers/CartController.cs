using Cinema.Identity;
using Cinema.Models;
using Cinema.Models.enums;
using Cinema.Repositories;
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
        private readonly IorderItemsRepo orderItemRepo;
        private readonly IorderRepo orderRepo;
        private readonly ICartItemsRepo cartItemRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMovieRepo movieRepo;

        public CartController(IcartRepo cartRepo, IorderItemsRepo orderItemRepo, IorderRepo orderRepo, ICartItemsRepo cartItemRepo, UserManager<ApplicationUser> userManager, IMovieRepo movieRepo)
        {
            this.cartRepo = cartRepo;
            this.orderItemRepo = orderItemRepo;
            this.orderRepo = orderRepo;
            this.cartItemRepo = cartItemRepo;
            this.userManager = userManager;
            this.movieRepo = movieRepo;
        }


        public IActionResult  AddToCart(int movieId, int Count)
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
            if (cart == null || cart.CartItems.Count == 0)
            {
                return View("NoCart");
            }

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

            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null || cartItem.CartId != cart.Id)
            {
                return BadRequest("Unauthorized action.");
            }

            cartItemRepo.Delete(cartItem);
            cartItemRepo.Commit();

            cartRepo.Delete(cart);
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
            var cartItem = cartItemRepo.GetOne(e=>e.Id==cartItemId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cartItemRepo.Edit(cartItem);
                }
                else
                {
                    cartItemRepo.Delete(cartItem);
                    TempData["SuccessMessage"] = "Movie removed from cart successfully!";
                }

                cartItemRepo.Commit(); 
            }

            return RedirectToAction("DisplayCart");
        }

        public IActionResult Checkout()
        {
            var user = userManager.GetUserId(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            var cart = cartRepo.GetCartByUserId(user);
            if (cart == null || cart.CartItems.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty!";
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserId = user,
                TotalAmount = cart.CartItems.Sum(e => e.Quantity * e.movie.Price),
                Status = OrderStatus.Pending,
            };

            orderRepo.Create(order);
            orderRepo.Commit();

            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItems
                {
                    OrderId = order.Id,
                    MovieId = item.MovieId,
                    Quantity = item.Quantity,
                    Price = item.movie.Price
                };
                orderItemRepo.Create(orderItem);
            }

            orderItemRepo.Commit(); 

            order = orderRepo.GetOne(e => e.Id == order.Id, includes: [e => e.OrderItems]);

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                TempData["ErrorMessage"] = "Order items were not saved!";
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            return RedirectToAction("Payment", new { orderId = order.Id });
        }


        public IActionResult Payment(int orderId)
        {
             var order = orderRepo.GetOrderWithDetails(orderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found!";
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                TempData["ErrorMessage"] = "Order items were not saved!";
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }
            StripeService stripeService = new StripeService();
            string sessionUrl = stripeService.CreateCheckoutSession(order, HttpContext);
            return Redirect(sessionUrl);
        }



        public IActionResult PaymentSuccess(int orderId)
        {
            var order = orderRepo.GetOne(e=>e.Id==orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = OrderStatus.Completed; 
            order.OrderDate = DateTime.Now; 
            orderRepo.Edit(order);
            orderRepo.Commit();

            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Customer" });
            }
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart != null)
            {
                cartRepo.ClearCart(cart);
                cartRepo.Commit();
                cartRepo.Delete(cart);
                cartItemRepo.Commit();
            }
            TempData["SuccessMessage"] = "Your payment was successful! Thank you.";
            return RedirectToAction("Index", "Movie", new { area = "Customer" });
        }

        public IActionResult PaymentFailed(int orderId)
        {
            TempData["ErrorMessage"] = "Payment was unsuccessful. Please try again.";
            return RedirectToAction("Checkout", "Cart");
        }

        public IActionResult OrderDetails(int orderId)
        {
            var order = orderRepo.GetOne(e => e.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }

}
