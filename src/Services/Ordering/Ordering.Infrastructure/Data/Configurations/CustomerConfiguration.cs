using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // fluent validation keşfedilmeden önce :D -- ama domain driven için sonuçta bu
        
        builder.HasKey(c => c.Id);
        // CustomerId nesnesini veritabanına yazarken ve okurken özel bir dönüşüm uygular -- gerek olmazdı eğer sadece guid olsaydı ama değil :D
        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            dbId => CustomerId.Of(dbId));
        
        builder.Property(c=> c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.HasIndex(c => c.Email).IsUnique();
    }
}