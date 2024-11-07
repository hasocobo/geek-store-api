using Microsoft.Build.Framework;
using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        [NotEmptyGuid]
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        [NotEmptyGuid]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
