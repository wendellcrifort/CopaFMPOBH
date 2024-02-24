using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly CopaContext _dbContext;
        protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

        public Repository(CopaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(TKey key)
        {
            return await _dbContext.FindAsync<TEntity>(key);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            this.DbSet.Add(entity);

            await this._dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            this._dbContext.Entry<TEntity>(entity).State = EntityState.Modified;

            await this._dbContext.SaveChangesAsync();

            return entity;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            this.DbSet.Remove(entity);

            this._dbContext.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return this.DbSet;
            }
        }

        public virtual IQueryable<TEntity> TableNoTracking
        {
            get
            {
                return this.DbSet.AsNoTracking();
            }
        }
    }
}
