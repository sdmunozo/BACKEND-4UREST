using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetShip.Entities;

namespace NetShip
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("MasterBase");

            modelBuilder.Entity<Category>().Property(p => p.Icon).IsUnicode();
            modelBuilder.Entity<Product>().Property(p => p.Icon).IsUnicode();

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.Entity<Brand>()
                        .HasOne(b => b.User)
                        .WithOne()
                        .HasForeignKey<Brand>("UserId");


            modelBuilder.Entity<PricePerModifierPerPlatform>().HasKey(p => new { p.PlatformId, p.ModifierId});
            modelBuilder.Entity<PricePerItemPerPlatform>().HasKey(p => new { p.PlatformId, p.ItemId });

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<ModifiersGroup> ModifiersGroups { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }

        public DbSet<PricePerModifierPerPlatform> PricePerModifierPerPlatforms { get; set; }
        public DbSet<PricePerItemPerPlatform> PricePerItemPerPlatforms { get; set; }
    }
}
