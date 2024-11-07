using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
        
        [StringLength(50)]
        public string? Sku { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least one.")]
        public int? Stock { get; set; }
        
        [NotEmptyGuid]
        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
