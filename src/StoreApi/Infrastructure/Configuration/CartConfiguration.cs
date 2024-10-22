using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasData(
                new Cart
                {
                    Id = new Guid("7D15AC84-2C94-4AD9-AFD3-D5A7D2A9FE1C"),
                    CustomerId = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"), // Hasan Çoban
                    ProductId = new Guid("886B55AE-3DD1-44FB-992C-20EF034134BF") // Wireless Mouse

                }
            );
        }
    }
}
