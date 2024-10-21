using System.ComponentModel.DataAnnotations;

namespace StoreApi.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category name field is required.")]
        public string? Name { get; set; }
    }
}
