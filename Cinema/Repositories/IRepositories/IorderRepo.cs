using Cinema.Models;

namespace Cinema.Repositories.IRepositories
{
    public interface IorderRepo :IRepository<Order>
    {
        Order? GetOrderWithDetails(int orderId);

    }
}
