using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinema.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepo
    {
        private readonly ApplicationDbContext dbContext;
        public ActorRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.dbContext = applicationDb;
        }
        public Actor? GetActorWithDetails(Expression<Func<Actor, bool>> filter)
        {
            return dbContext.actors
                .Include(a => a.ActorMovies)
                    .ThenInclude(am => am.Movie)
                .FirstOrDefault(filter);
        }

    }
}
