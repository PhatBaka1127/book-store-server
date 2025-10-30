using BookStore.Data.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Helper
{
    public static class LinQUtils
    {
        public static IQueryable<TEntity> DynamicFilter<TEntity, TFilter>(
                this IQueryable<TEntity> source, TFilter filter)
        {
            if (filter == null) return source;

            var filterProps = typeof(TFilter).GetProperties();
            var entityProps = typeof(TEntity).GetProperties().Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in filterProps)
            {
                var val = prop.GetValue(filter);
                if (val == null) continue;

                bool hasSortAttr = prop.CustomAttributes.Any(a => a.AttributeType == typeof(SortAttribute));

                if (!entityProps.Contains(prop.Name) && !hasSortAttr) continue;

                if (prop.CustomAttributes.Any(a => a.AttributeType == typeof(SkipAttribute))) continue;

                if (val is DateTime dt)
                {
                    source = source.Where($"{prop.Name} >= @0 && {prop.Name} < @1", dt.Date, dt.Date.AddDays(1));
                    continue;
                }

                if (hasSortAttr)
                {
                    var sortParts = val.ToString()?.Split(',', StringSplitOptions.TrimEntries);
                    if (sortParts == null || sortParts.Length == 0) continue;

                    var sortField = sortParts[0];
                    var direction = sortParts.Length > 1 ? sortParts[1].ToLower() : "asc";

                    if (!entityProps.Contains(sortField)) continue;

                    source = direction == "desc"
                        ? source.OrderBy($"{sortField} descending")
                        : source.OrderBy($"{sortField}");
                    continue;
                }

                if (val is string s)
                {
                    source = source.Where($"{prop.Name}.ToLower().Contains(@0)", s.Trim().ToLower());
                    continue;
                }

                source = source.Where($"{prop.Name} == @0", val);
            }

            return source;
        }

        public static (int, IQueryable<TResult>) PagingIQueryable<TResult>(this IQueryable<TResult> source, int page, int size, int limitPaging, int defaultPaging)
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
