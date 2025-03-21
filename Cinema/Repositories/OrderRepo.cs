using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class OrderRepo :Repository<Order>, IorderRepo
    {
        private readonly ApplicationDbContext applicationDb;
        public OrderRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.applicationDb = applicationDb;
        }
        public Order? GetOrderWithDetails(int orderId)
        {
            return applicationDb.Orders
                .Include(o => o.OrderItems)  // Load OrderItems
                .ThenInclude(oi => oi.Movie) // Load Movie data for each OrderItem
                .FirstOrDefault(o => o.Id == orderId);
        }


    }
}
