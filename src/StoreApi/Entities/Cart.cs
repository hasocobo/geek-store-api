using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
