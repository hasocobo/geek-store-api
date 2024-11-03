using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Entities;

namespace StoreApi.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = new Guid("000C06FB-FD88-4661-BBBA-9AFEFE9CCC3F").ToString(),
                    FirstName = "Hasan",
                    LastName = "Çoban",
                    UserName = "hasancoban",
                    Email = "hasancoban@std.iyte.edu.tr"
                },
                new User
                {
                    Id = new Guid("111C06FB-FD88-4661-BBBA-9AFEFE9CCC3F").ToString(),
                    FirstName = "Bora",
                    LastName = "Ersoy",
                    UserName = "boraersoy",
                    Email = "boraersoy@std.iyte.edu.tr",
                }
            );
        }
    }
}