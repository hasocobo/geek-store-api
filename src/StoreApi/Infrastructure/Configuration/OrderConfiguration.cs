using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasData(
                new Order
                {
                    Id = new Guid("0AA7B22D-C50F-477E-93DF-822E2AD2C264"),
                    CustomerId = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"), // Hasan Çoban
                    Date = new DateTime(2024, 10, 21),
                    TotalPrice = 55.00m
                },
                new Order
                {
                    Id = new Guid("10AF2D9B-2007-4798-BE1C-115C852C2FFE"),
                    CustomerId = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"), // Hasan Çoban
                    Date = new DateTime(2024, 10, 21),
                    TotalPrice = 125.00m
                }
            );
        }
    }
}
