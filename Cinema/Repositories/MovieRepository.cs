using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinema.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepo
    {
        private readonly ApplicationDbContext dbContext;
        public MovieRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.dbContext = applicationDb;
        }

        public Movie? GetMovieWithDetails(Expression<Func<Movie, bool>> filter)
        {
            return dbContext.movies
                .Include(m => m.Cinema)
                .Include(m => m.Category)
                .Include(m => m.ActorMovies)
                    .ThenInclude(am => am.Actor)
                .FirstOrDefault(filter);
        }

    }
}
