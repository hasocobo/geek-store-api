﻿namespace StoreApi.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }
    }
}
