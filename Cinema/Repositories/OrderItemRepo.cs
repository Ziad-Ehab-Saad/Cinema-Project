using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class OrderItemRepo :Repository<OrderItems>, IorderItemsRepo
    {
        private readonly ApplicationDbContext applicationDb;

        public OrderItemRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.applicationDb = applicationDb;
        }
    }
}
