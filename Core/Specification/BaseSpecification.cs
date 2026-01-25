using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{

    public BaseSpecification() : this(null)
    {

    }
    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set; }

    public bool IsDistinct { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginationEnable { get; private set; }

    public List<Expression<Func<T, object>>> Includes { get; } = [];

    public List<string> IncludeStrings { get; } = [];

    public void AddInclude(Expression<Func<T, object>> expression)
    {
        Includes.Add(expression);
    }

    public void AddInclude(string IncludeString)
    {
        IncludeStrings.Add(IncludeString);
    }

    public void AddOrderBy(Expression<Func<T, object>> expression)
    {
        OrderBy = expression;
    }
    public void AddOrderByDesc(Expression<Func<T, object>> expression)
    {
        OrderByDesc = expression;
    }
    //Use for method like Count, cuz we only want to filter by criteria, not paging here 
    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (Criteria != null)
        {
            query = query.Where(Criteria);
        }

        return query;
    }

    public void ApplyInstinct()
    {
        IsDistinct = true;
    }

    public void ApplyPaging(int take, int skip)
    {
        IsPaginationEnable = true;
        Take = take;
        Skip = skip;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    public BaseSpecification() : this(null)
    {

    }
    public Expression<Func<T, TResult>>? Select { get; private set; }
    protected void AddSelete(Expression<Func<T, TResult>> expression)
    {
        Select = expression;
    }
}