using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }
    
    public DbSet<Coupon> Coupons { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon {Id = 1,ProductName = "Iphone 15",Description = "Newest Iphone so far",Amount = 100},
            new Coupon {Id = 2,ProductName = "Samsung Galaxy S24",Description = "Explore the galaxy series",Amount = 150});
    }
}