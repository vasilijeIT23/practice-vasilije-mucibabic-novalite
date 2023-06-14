using Microsoft.EntityFrameworkCore;
using MovieStoreCore;
using MovieStoreInfrastructure;
using System.Linq.Expressions;

namespace MovieStoreApi.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(MovieStoreContext context) : base(context)
        {
        }
        public override IEnumerable<Customer> GetAll() => dbSet.Include(x => x.PurchasedMovies).ThenInclude(x => x.Movie);
        public override Customer? GetById(Guid id) => dbSet.Include(x => x.PurchasedMovies).ThenInclude(x => x.Movie).FirstOrDefault(x => x.Id == id);

        public override IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate) => dbSet.Include(x => x.PurchasedMovies).ThenInclude(x => x.Movie).Where(predicate);
    }
}
