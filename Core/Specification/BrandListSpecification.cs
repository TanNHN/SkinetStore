using Core.Entities;

namespace Core.Specification;

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelete(x => x.Brand);
        ApplyInstinct();
    }
}
