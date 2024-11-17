using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Common.ModelBinders;

namespace StoreApi.Common.QueryFeatures;

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

    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;

    [ModelBinder(BinderType = typeof(FiltersModelBinder))]
    public List<Filter> Filters { get; set; } = new();

    public string? SearchTerm { get; set; }
}