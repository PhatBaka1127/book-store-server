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
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity?> FindAsync(Guid id, bool isTracking = false);
        Task<TEntity?> GetByIdAsync(object id,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null,
                                        bool isTracking = false);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null,
                                                bool isTracking = false);
        Task<IEnumerable<TEntity>?> GetWhereAsync(Expression<Func<TEntity, bool>> filter,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null,
                                                    bool isTracking = false);
        IQueryable<TEntity> GetTable();
        Task<bool> SaveChanges();
    }
}
