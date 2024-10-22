﻿namespace StoreApi.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
