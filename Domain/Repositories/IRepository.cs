namespace Domain.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();  
        Task<TEntity> GetByIdAsync(TKey key);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }
    }
}
