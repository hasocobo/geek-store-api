using System.ComponentModel.DataAnnotations;

namespace StoreApi.Common.QueryFeatures;

/**
 * The reason why I chose a custom filter record over a dictionary is
 * to allow duplicate filtering requests like ?Filters=Color:Blue;Color:Red.
 */
public record Filter
{
    [Required] public string Key { get; set; }

    [Required] public string Value { get; set; }
};