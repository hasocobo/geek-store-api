﻿namespace StoreApi.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
    }
}
