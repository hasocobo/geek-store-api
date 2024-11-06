using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StoreApi.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly StoreContext _storeContext;

        protected RepositoryBase(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public void Create(T entity)
        {
            _storeContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _storeContext.Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return _storeContext.Set<T>();
        }
        public void Update(T entity)
        {
            _storeContext.Set<T>().Update(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {

            return _storeContext.Set<T>().Where(expression);
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            return await _storeContext.Set<T>().AnyAsync(expression);
        }

    }
}
