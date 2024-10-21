using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }

    }
}
