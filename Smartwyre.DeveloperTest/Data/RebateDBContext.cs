using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    public class RebateDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Rebate> Rebates { get; set; }
        public DbSet<RebateCalculation> RebateCalculations { get; set; }

        public RebateDbContext(DbContextOptions<RebateDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rebate>()
                .Property(r => r.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rebate>()
                .Property(r => r.Percentage)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RebateCalculation>()
                .Property(rc => rc.Amount)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
