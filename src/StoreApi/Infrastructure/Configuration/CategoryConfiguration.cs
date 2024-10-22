using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = new Guid("73D0B378-6A25-4919-B2E7-4C3E9ED95695"),
                    Name = "Electronics",
                }
            );
        }
    }
}
