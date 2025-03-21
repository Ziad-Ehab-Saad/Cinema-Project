using Cinema.Models;

namespace Cinema.Repositories.IRepositories
{
    public interface IcartRepo : IRepository<Cart>
    {
        public Cart GetCartByUserId(string userId);
        public void ClearCart(Cart cart);
    }
}
