using Cinema.DataAccess;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Linq;
using System.Linq.Expressions;

namespace Cinema.Repositories
{
    public class CategoryRepo : Repository<Category>,ICategoryrepo
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            this.dbContext = applicationDb;
        }

        public Category? GetCategoryWithDetails(Expression<Func<Category, bool>> filter)
        {
            return dbContext.categories
                .Include(c => c.Movies)
                .ThenInclude(m=>m.Cinema)
                .FirstOrDefault(filter);


        }

        public void Update(Category category) {
        dbContext.categories.Update(category);

        }
    }
}
