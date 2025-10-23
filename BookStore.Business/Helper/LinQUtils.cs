using BookStore.Data.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Helper
{
    public static class LinQUtils
    {
        public static (int, IQueryable<TResult>) PagingIQueryable<TResult>(this IQueryable<TResult> source, int page, int size,  int limitPaging, int defaultPaging)
        {
            try
            {
                if (size > limitPaging)
                {
                    size = limitPaging;
                }
                if (size < 1)
                {
                    size = defaultPaging;
                }
                if (page < 1)
                {
                    page = 1;
                }
                int total = source == null ? 0 : source.Count();
                IQueryable<TResult> results = source
                    .Skip((page - 1) * size)
                    .Take(size);
                return (total, results);
            }
            catch (Exception ex)
            {
                LoggerService.Logger(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
