namespace StoreApi.Infrastructure
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAllAsync();
        Task<IQueryable> FindByConditionAsync();   
    }
}
