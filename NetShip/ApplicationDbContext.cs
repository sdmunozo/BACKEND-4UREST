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

            modelBuilder.Entity<LandingUserEvent>(entity =>
            {
                entity.ToTable("LandingUserEvents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SessionId).IsRequired();
                entity.Property(e => e.EventType).IsRequired();
                entity.Property(e => e.EventTimestamp).HasColumnType("datetime");

                entity.OwnsOne(e => e.Details, details =>
                {
                    details.Property(d => d.PresentationViewSecondsElapsed).HasColumnName("PresentationViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.MenuHighlightsViewSecondsElapsed).HasColumnName("MenuHighlightsViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.MenuScreensViewSecondsElapsed).HasColumnName("MenuScreensViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.ForWhoViewSecondsElapsed).HasColumnName("ForWhoViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.WhyUsViewSecondsElapsed).HasColumnName("WhyUsViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.SuscriptionsViewSecondsElapsed).HasColumnName("SuscriptionsViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.TestimonialsViewSecondsElapsed).HasColumnName("TestimonialsViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.FaqViewSecondsElapsed).HasColumnName("FaqViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.TrustElementsViewSecondsElapsed).HasColumnName("TrustElementsViewSecondsElapsed").IsRequired(false);
                    details.Property(d => d.LinkDestination).HasColumnName("LinkDestination").HasMaxLength(256).IsRequired(false);
                    details.Property(d => d.LinkLabel).HasColumnName("LinkLabel").HasMaxLength(256).IsRequired(false);
                    details.Property(d => d.PlaybackTime).HasColumnName("PlaybackTime").IsRequired(false);
                    details.Property(d => d.Duration).HasColumnName("Duration").IsRequired(false);
                    details.Property(d => d.ImageId).HasColumnName("ImageId").HasMaxLength(128).IsRequired(false);
                    details.Property(d => d.FAQId).HasColumnName("FAQId").HasMaxLength(128).IsRequired(false);
                    details.Property(d => d.Status).HasColumnName("Status").HasMaxLength(50).IsRequired(false);
                });
            });

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

            modelBuilder.Entity<PricePerModifierPerPlatform>().HasKey(p => new { p.PlatformId, p.ModifierId });
            /* modelBuilder.Entity<PricePerItemPerPlatform>().HasKey(p => new { p.PlatformId, p.ItemId });

             modelBuilder.Entity<PricePerItemPerPlatform>()
                 .HasOne<Item>(p => p.Item)
                 .WithMany()
                 .HasForeignKey(p => p.ItemId)
                 .OnDelete(DeleteBehavior.Restrict);*/
            modelBuilder.Entity<PricePerItemPerPlatform>()
     .HasKey(p => p.Id); // Usa Id como PK principal

            modelBuilder.Entity<PricePerItemPerPlatform>()
                .HasIndex(p => new { p.PlatformId, p.ItemId })
                .IsUnique(); // Si es necesario

            modelBuilder.Entity<PricePerItemPerPlatform>()
                .HasOne(p => p.Platform)
                .WithMany()
                .HasForeignKey(p => p.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PricePerItemPerPlatform>()
                .HasOne(p => p.Item)
                .WithMany()
                .HasForeignKey(p => p.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PricePerItemPerPlatform>()
                .HasOne<Platform>(p => p.Platform)
                .WithMany()
                .HasForeignKey(p => p.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PricePerModifierPerPlatform>()
                .HasOne<Modifier>(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.ModifierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PricePerModifierPerPlatform>()
                .HasOne<Platform>(p => p.Platform)
                .WithMany()
                .HasForeignKey(p => p.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
    .Property(p => p.Price)
    .HasPrecision(10, 2);

            modelBuilder.Entity<Modifier>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<LandingUserEvent> LandingUserEvents { get; set; }


        public DbSet<Platform> Platforms { get; set; }
        public DbSet<ModifiersGroup> ModifiersGroups { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<DeviceTracking> DeviceTrackings { get; set; }

        public DbSet<PricePerModifierPerPlatform> PricePerModifierPerPlatforms { get; set; }
        public DbSet<PricePerItemPerPlatform> PricePerItemPerPlatforms { get; set; }
    }
}









/*
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

        } */
