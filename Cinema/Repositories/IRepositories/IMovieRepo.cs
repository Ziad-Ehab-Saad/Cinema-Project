using Cinema.Models;
using System.Linq.Expressions;

namespace Cinema.Repositories.IRepositories
{
    public interface IMovieRepo : IRepository<Movie>
    {
        Movie? GetMovieWithDetails(Expression<Func<Movie, bool>> filter);
        public void Update(Movie movie);
    }
}
