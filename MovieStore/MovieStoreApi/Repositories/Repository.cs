using Microsoft.EntityFrameworkCore;
using MovieStoreInfrastructure;
using System.Data;
using System.Linq.Expressions;

namespace MovieStoreApi.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected MovieStoreContext context;
        protected DbSet<T> dbSet;

        public Repository(MovieStoreContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => context.Set<T>().AsEnumerable();

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => dbSet.Where(predicate).AsEnumerable();

        public virtual T? GetById(Guid id) => dbSet.Find(id);

        public virtual void Create(T entity) => dbSet.Add(entity);

        public virtual void Delete(T entity) => dbSet.Remove(entity);

        public virtual bool SaveChanges() => context.SaveChanges() > 0;
    }
}
