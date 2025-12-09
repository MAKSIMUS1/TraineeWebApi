using Microsoft.EntityFrameworkCore;
using WebApiTrainingProject.Repositories.Interfaces;

namespace WebApiTrainingProject.Repositories
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
            where TEntity : class
            where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Guid id) =>
            await _dbSet.FindAsync(id);

        public async Task<TEntity?> AddAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entry.Entity;
        }

        public Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var entry = _dbSet.Update(entity);
            return Task.FromResult(entry.Entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }
    }
}
