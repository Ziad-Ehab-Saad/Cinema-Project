using Cinema.Models;
using System.Linq.Expressions;

namespace Cinema.Repositories.IRepositories
{
    public interface ICategoryrepo : IRepository<Category>
    {
        public Category? GetCategoryWithDetails(Expression<Func<Category, bool>> filter);
        public void Update(Category category);
    }
}
