using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasData(
                new OrderItem
                {
                    Id = new Guid("1AA7B22D-C50F-477E-93DF-822E2AD2C264"),
                    OrderId = new Guid("0AA7B22D-C50F-477E-93DF-822E2AD2C264"), //Guid of order 1
                    ProductId = new Guid("17892431-77A6-4A94-9FAC-70A05552AA8F"), //Mechanical Keyboard

                },
                new OrderItem
                {
                    Id = new Guid("2AAF2D9B-2007-4798-BE1C-115C852C2FFE"),
                    OrderId = new Guid("0AA7B22D-C50F-477E-93DF-822E2AD2C264"), //Guid of order 1
                    ProductId = new Guid("886B55AE-3DD1-44FB-992C-20EF034134BF"), //Wireless Mouse

                },
                new OrderItem
                {
                    Id = new Guid("3AAF2D9B-2007-4798-BE1C-115C852C2FFE"),
                    OrderId = new Guid("10AF2D9B-2007-4798-BE1C-115C852C2FFE"), //Guid of order 2
                    ProductId = new Guid("17892431-77A6-4A94-9FAC-70A05552AA8F"), //Mechanical Keyboard

                },
                new OrderItem
                {
                    Id = new Guid("4AAF2D9B-2007-4798-BE1C-115C852C2FFE"),
                    OrderId = new Guid("10AF2D9B-2007-4798-BE1C-115C852C2FFE"), //Guid of order 2
                    ProductId = new Guid("AE450822-2DDF-4CA3-8476-8849B6041939"), //USB-C Hub

                }
            );
        }
    }
}
