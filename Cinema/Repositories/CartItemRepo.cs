using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class CartItemRepo :Repository<CartItems>, ICartItemsRepo
    {
        private readonly ApplicationDbContext dbContext;

        public CartItemRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public CartItems GetCartItem(int cartId, int movieId)
        {
            return dbContext.cartItems.FirstOrDefault(ci => ci.CartId == cartId && ci.MovieId == movieId);
        }
    }
}
