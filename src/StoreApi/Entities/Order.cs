namespace StoreApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
    }
}
