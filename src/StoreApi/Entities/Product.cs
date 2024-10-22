using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product name is a required field.")]

        public string? SKU { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        public ICollection<Wishlist> Wishlists { get; set; }
            = new List<Wishlist>();

        public ICollection<Cart> Carts { get; set;}
            = new List<Cart>();
    }
}
