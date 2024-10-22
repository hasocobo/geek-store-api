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
                    Id = Guid.NewGuid(),
                    Name = "Wireless Mouse",
                    SKU = "WM-001",
                    Description = "A smooth and responsive wireless mouse for everyday use.",
                    Price = 29.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Mechanical Keyboard",
                    SKU = "MK-002",
                    Description =
                        "A high-quality mechanical keyboard with customizable RGB lighting.",
                    Price = 79.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "USB-C Hub",
                    SKU = "UH-003",
                    Description = "A versatile USB-C hub with multiple ports for connectivity.",
                    Price = 49.99m,
                    CategoryId = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"),
                }
            );
        }
    }
}
