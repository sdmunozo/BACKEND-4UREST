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


            modelBuilder.Entity<Category>().Property( p => p.Name).HasMaxLength(50);
        }

        public DbSet<Category> Categories { get; set; }
    }
}
