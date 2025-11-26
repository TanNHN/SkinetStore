namespace Core.Specification;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get { return _pageSize; }
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        set
        {
            _brands = value
                .SelectMany(b => b.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set
        {
            _types = value
                .SelectMany(t => t.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    public string? Sort { get; set; }
    private string _search;
    public string Search
    {
        get { return _search ?? ""; }
        set { _search = value.ToLower(); }
    }

}
