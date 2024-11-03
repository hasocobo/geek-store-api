using System.ComponentModel.DataAnnotations;

namespace StoreApi.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}