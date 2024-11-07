using System.ComponentModel.DataAnnotations;

namespace StoreApi.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
