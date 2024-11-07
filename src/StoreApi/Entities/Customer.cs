using System.ComponentModel.DataAnnotations;

namespace StoreApi.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        [StringLength(128)]
        public required string UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}