using System.ComponentModel.DataAnnotations;

namespace StoreApi.Common.ValidationAttributes;

public class NotEmptyGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            return new ValidationResult("Field cannot be an empty Guid.");
        }
        return ValidationResult.Success;
    }
}