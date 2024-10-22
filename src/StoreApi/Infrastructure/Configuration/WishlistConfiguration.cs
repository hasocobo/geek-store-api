using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasData(
                new Wishlist
                {
                    Id = new Guid("797C646A-C836-43DB-B232-742254D03BF0"),
                    CustomerId = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"), // Hasan Çoban
                    ProductId = new Guid("17892431-77A6-4A94-9FAC-70A05552AA8F"), //Mechanical Keyboard
                },
                new Wishlist
                {
                    Id = new Guid("697C646A-C836-43DB-B232-742254D03BF0"),
                    CustomerId = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"), // Hasan Çoban
                    ProductId = new Guid("AE450822-2DDF-4CA3-8476-8849B6041939") // USB-C Hub
                }
            );
        }
    }
}
