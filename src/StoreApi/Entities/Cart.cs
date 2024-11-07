using System.ComponentModel.DataAnnotations;
using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be at least 1")] 
        public int Quantity { get; set; }
        
        [NotEmptyGuid]
        public Guid CustomerId { get; set; }
        
        public Customer? Customer { get; set; }
        
        [NotEmptyGuid]
        public Guid ProductId { get; set; }
        
        public Product? Product { get; set; }
    }
}