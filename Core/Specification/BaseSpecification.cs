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

    public void AddOrderBy(Expression<Func<T, object>> expression)
    {
        OrderBy = expression;
    }
    public void AddOrderByDesc(Expression<Func<T, object>> expression)
    {
        OrderByDesc = expression;
    }
    public void ApplyInstinct()
    {
        IsDistinct = true;
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