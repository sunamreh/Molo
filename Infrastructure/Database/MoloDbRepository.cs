using Microsoft.EntityFrameworkCore;
using Molo.Application.Common.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Molo.Infrastructure.Database
{
    public class MoloDbRepository<TEntity> : IMoloDbRepository<TEntity> where TEntity : class
    {
        private readonly MoloDbContext _context;
        private readonly DbSet<TEntity> _set;

        public MoloDbRepository(MoloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _set.ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, 
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set.Where(predicate);

            return await includes.Aggregate(query, (current, includeProperty) => 
                current.Include(includeProperty)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set.Where(predicate);

            return await includes.Aggregate(query, (current, includeProperty) =>
                current.Include(includeProperty)).ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _set.FindAsync(id);
        }

        public async Task Add(TEntity entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
