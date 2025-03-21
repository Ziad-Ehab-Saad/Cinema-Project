using Cinema.Models;

namespace Cinema.Repositories.IRepositories
{
    public interface ICartItemsRepo : IRepository<CartItems>
    {
        public CartItems GetCartItem(int cartId, int movieId);
    }
}
