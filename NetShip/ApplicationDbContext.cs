using Microsoft.EntityFrameworkCore;
using NetShip.Entities;

namespace NetShip
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("MasterBase");

            //modelBuilder.Entity<Category>().Property( p => p.Name).HasMaxLength(50);

            modelBuilder.Entity<Category>().Property(p => p.Icon).IsUnicode();
            modelBuilder.Entity<Product>().Property(p => p.Icon).IsUnicode();

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
