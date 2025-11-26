using System;
using Core.Entities;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParams)
        : base(x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.Contains(specParams.Search)) && 
            //If brand IsNullOrEmpty = true, code will ignore 2nd condition => no  brand filter provide
            (!specParams.Brands.Any() || specParams.Brands.Contains(x.Brand))
            && (!specParams.Types.Any() || specParams.Types.Contains(x.Type))
        )
    {
        ApplyPaging(specParams.PageSize, specParams.PageSize * (specParams.PageIndex - 1));
        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDesc(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
