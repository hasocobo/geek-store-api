using Microsoft.AspNetCore.Mvc.ModelBinding;
using StoreApi.Common.QueryFeatures;

namespace StoreApi.Common.ModelBinders;

public class FiltersModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var filtersValue =
            bindingContext.ValueProvider.GetValue("Filters").FirstValue; // e.g Filters = {"Color:Blue;Brand:Nike"}

        if (string.IsNullOrEmpty(filtersValue))
        {
            bindingContext.Result = ModelBindingResult.Success(new List<Filter>());
            return Task.CompletedTask;
        }

        var filters = filtersValue.Split(";", StringSplitOptions.RemoveEmptyEntries)
            .Select<string, Filter>(filter =>
            {
                var parts = filter.Split(":", StringSplitOptions.RemoveEmptyEntries);
                return new Filter
                {
                    Key = parts[0],
                    Value = parts[1]
                };
            }).ToList();
        
        bindingContext.Result = ModelBindingResult.Success(filters);
        return Task.CompletedTask;
    }
}