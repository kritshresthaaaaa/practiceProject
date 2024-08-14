
using Domains.Models;
using Domains.Models.BridgeEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, ISaveChangesInterceptor
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
        }

    }
}
