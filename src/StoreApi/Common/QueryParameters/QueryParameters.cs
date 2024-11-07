using System.ComponentModel.DataAnnotations;

namespace StoreApi.Common.QueryParameters;

public class QueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string SortBy { get; set; } = "Id";
    public bool SortDescending { get; set; } = false;
}