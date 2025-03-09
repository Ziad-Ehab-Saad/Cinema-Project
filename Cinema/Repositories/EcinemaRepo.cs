using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class EcinemaRepo : Repository<ECinema>, IEcinemaRepo
    {
        private readonly ApplicationDbContext applicationDb;

        public EcinemaRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.applicationDb = applicationDb;
        }
    }
}
