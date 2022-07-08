using DDDMart.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Infrastructure.Repositories
{
    public abstract class Repository<T, TContext> : IRepository<T> where T : AggregateRoot where TContext : DbContextBase<TContext>
    {
        private readonly DbContextBase<TContext> _context;
        private readonly DbSet<T> _entitySet;

        protected Repository(DbContextBase<TContext> context)
        {
            _context = context;
            // EnsureCreated is being used here for testing purposes
            // and shouldn't be used in Production code
            _context.Database.EnsureCreated();
            _entitySet = _context.Set<T>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public IQueryable<T> GetAll(bool noTracking = true)
        {
            var set = _entitySet;
            if (noTracking)
            {
                return set.AsNoTracking();
            }
            return set;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await GetAll(false)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(T entity)
        {
            await _entitySet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _entitySet.Remove(entity);
        }

        public void Remove(IEnumerable<T> entitiesToRemove)
        {
            _entitySet.RemoveRange(entitiesToRemove);
        }
    }
}
