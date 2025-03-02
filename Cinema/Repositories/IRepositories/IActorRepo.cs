using Cinema.Models;
using System.Linq.Expressions;

namespace Cinema.Repositories.IRepositories
{
    public interface IActorRepo :  IRepository<Actor>
    {
        Actor? GetActorWithDetails(Expression<Func<Actor, bool>> filter);


    }
}
