using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class CartRepo : Repository<Cart> , IcartRepo
    {
        private readonly ApplicationDbContext dbContext;
        public CartRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.dbContext = applicationDb;
        }
           public Cart GetCartByUserId(string userId)
        {
            return dbContext.carts
                .Include(c => c.CartItems).ThenInclude(e=>e.movie) 
                .FirstOrDefault(c => c.UserId == userId);

    }
        public void ClearCart(Cart cart)
        {
            cart.CartItems.Clear(); // Remove all items
            Commit(); // Save changes
        }

    }
}
