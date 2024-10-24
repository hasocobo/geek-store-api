using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = new Guid("886B55AE-3DD1-44FB-992C-20EF034134BF"),
                    Name = "Wireless Mouse",
                    Sku = "WM-001",
                    Description = "A smooth and responsive wireless mouse for everyday use.",
                    Price = 29.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"), //Electronics
                },
                new Product
                {
                    Id = new Guid("17892431-77A6-4A94-9FAC-70A05552AA8F"),
                    Name = "Mechanical Keyboard",
                    Sku = "MK-002",
                    Description =
                        "A high-quality mechanical keyboard with customizable RGB lighting.",
                    Price = 79.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"), //Electronics
                },
                new Product
                {
                    Id = new Guid("AE450822-2DDF-4CA3-8476-8849B6041939"), 
                    Name = "USB-C Hub",
                    Sku = "UH-003",
                    Description = "A versatile USB-C hub with multiple ports for connectivity.",
                    Price = 49.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"), //Electronics
                }
            );
        }
    }
}
