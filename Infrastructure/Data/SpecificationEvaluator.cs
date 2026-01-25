using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDesc != null)
        {
            query = query.OrderByDescending(spec.OrderByDesc);
        }
        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }
        if (spec.IsPaginationEnable)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }
        if (spec.Includes.Count > 0)
        {
            foreach (Expression<Func<T, object>> item in spec.Includes)
            {
                query = query.Include(item);
            }
        }
        if (spec.IncludeStrings.Count > 0)
        {
            foreach (string item in spec.IncludeStrings)
            {
                query = query.Include(item);
            }
        }
        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDesc != null)
        {
            query = query.OrderByDescending(spec.OrderByDesc);
        }
        var selectQuery = query as IQueryable<TResult>;
        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }
        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }
        if (spec.IsPaginationEnable)
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }
        return selectQuery ?? query.Cast<TResult>();
    }
}
