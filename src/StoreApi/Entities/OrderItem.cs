using System.ComponentModel.DataAnnotations;
using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be at least 1.")]
        public int Quantity { get; set; }
        // Including price here adds redundancy but it's useful in case of a price
        // change in the future
        
        [Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; } 
        
        [NotEmptyGuid]
        public Guid ProductId { get; set; }
        
        public Product? Product { get; set; }
        
        [NotEmptyGuid]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
