using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class ActorMovieRepo:Repository<ActorMovie>, IActorMovieRepo
    {
        public ActorMovieRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }

    }
}
