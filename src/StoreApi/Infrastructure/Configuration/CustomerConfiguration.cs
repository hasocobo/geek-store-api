using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasData(
                new Customer
                {
                    Id = new Guid("860C06FB-FD88-4661-BBBA-9AFEFE9CCC3F"),
                    Name = "Hasan Çoban",
                    Address = "İzmir",
                    Email = "hasancoban@std.iyte.edu.tr",
                    PhoneNumber = "5316731512",
                },
                new Customer
                {
                    Id = new Guid("4D4D5339-B460-4AFE-A131-9DE34DE8EE69"),
                    Name = "Bora Ersoy",
                    Address = "İzmir",
                    Email = "boraersoy@std.iyte.edu.tr",
                    PhoneNumber = "5416731512",
                }
            );
        }
    }
}
