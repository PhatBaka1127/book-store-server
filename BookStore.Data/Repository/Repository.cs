using BookStore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BookStoreContext _context;
        private readonly DbSet<TEntity> _table;

        public Repository(BookStoreContext context)
        {
            _context = context;
            _table = context.Set<TEntity>();
        }

        public void Add(TEntity entity) => _table.AddAsync(entity);

        public void Update(TEntity entity) => _table.Update(entity);

        public void Delete(TEntity entity) =>  _table.Remove(entity);

        public async Task<TEntity?> FindAsync(object id, bool isTracking = false)
        {
            try
            {
                var query = isTracking ? _table : _table.AsNoTracking();
                return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id") == id);
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<TEntity?> GetByIdAsync(object id,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null,
                                                    bool isTracking = false)
        {
            try
            {
                IQueryable<TEntity> query = isTracking ? _table : _table.AsNoTracking();
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
                return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null,
                                                            bool isTracking = false)
        {
            try
            {
                IQueryable<TEntity> query = isTracking ? _table : _table.AsNoTracking();
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
                return await query.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<TEntity>?> GetWhereAsync(Expression<Func<TEntity, bool>> filter,
                                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null,
                                                                  bool isTracking = false)
        {
            try
            {
                IQueryable<TEntity> query = isTracking ? _table : _table.AsNoTracking();
                if (includeProperties != null)
                {
                    query = includeProperties(query);
                }
                return await query.Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public IQueryable<TEntity> GetTable()
        {
            return _table;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
