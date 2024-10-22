namespace StoreApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
