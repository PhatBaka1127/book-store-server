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

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _table.AddAsync(entity);
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Update(entity);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                _table.Remove(entity);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.InnerException?.Message);
                throw new Exception(ex.InnerException?.Message);
            }
        }

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
    }
}
