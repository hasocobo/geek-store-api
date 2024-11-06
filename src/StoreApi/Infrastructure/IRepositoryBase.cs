using System.Linq.Expressions;

namespace StoreApi.Infrastructure
{
    public interface IRepositoryBase<T>
    {
        public IQueryable<T> FindAll();
        /// <summary>
        /// Makes a query to the database using the 
        /// provided expression
        /// 
        /// e.g: _customerRepository.FindByCondition(c => c.Id = givenId)
        /// this is queried as DbSet.Where(c => c.Id = givenId)
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        
        public Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}
