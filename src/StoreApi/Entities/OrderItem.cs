namespace StoreApi.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        // Including price here adds redundancy but it's useful in case of a price
        // change in the future
        public decimal Price { get; set; } 
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
