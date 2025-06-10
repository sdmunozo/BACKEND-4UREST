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

            // Configure new entities for tablet system
            // Waiter
            modelBuilder.Entity<Waiter>()
                .HasOne(w => w.Branch)
                .WithMany()
                .HasForeignKey(w => w.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Area
            modelBuilder.Entity<Area>()
                .HasOne(a => a.Branch)
                .WithMany()
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            modelBuilder.Entity<Table>()
                .HasOne(t => t.Area)
                .WithMany(a => a.Tables)
                .HasForeignKey(t => t.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Table>()
                .HasOne(t => t.CurrentSession)
                .WithOne()
                .HasForeignKey<Table>(t => t.CurrentSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // TableSession
            modelBuilder.Entity<TableSession>()
                .HasOne(ts => ts.Table)
                .WithMany(t => t.Sessions)
                .HasForeignKey(ts => ts.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TableSession>()
                .HasOne(ts => ts.Waiter)
                .WithMany(w => w.TableSessions)
                .HasForeignKey(ts => ts.WaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Branch)
                .WithMany()
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => new { c.BranchId, c.PhoneNumber })
                .IsUnique();

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Waiter)
                .WithMany(w => w.Orders)
                .HasForeignKey(o => o.WaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Session)
                .WithMany(ts => ts.Orders)
                .HasForeignKey(o => o.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany()
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItemModifier
            modelBuilder.Entity<OrderItemModifier>()
                .HasOne(oim => oim.OrderItem)
                .WithMany(oi => oi.Modifiers)
                .HasForeignKey(oim => oim.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItemModifier>()
                .HasOne(oim => oim.Modifier)
                .WithMany()
                .HasForeignKey(oim => oim.ModifierId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequest
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Table)
                .WithMany(t => t.ServiceRequests)
                .HasForeignKey(sr => sr.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Waiter)
                .WithMany(w => w.ServiceRequests)
                .HasForeignKey(sr => sr.WaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.RespondedByWaiter)
                .WithMany()
                .HasForeignKey(sr => sr.RespondedByWaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.ProcessedByWaiter)
                .WithMany()
                .HasForeignKey(p => p.ProcessedByWaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            // LoyaltyTransaction
            modelBuilder.Entity<LoyaltyTransaction>()
                .HasOne(lt => lt.Customer)
                .WithMany(c => c.LoyaltyTransactions)
                .HasForeignKey(lt => lt.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoyaltyTransaction>()
                .HasOne(lt => lt.Order)
                .WithMany()
                .HasForeignKey(lt => lt.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoyaltyTransaction>()
                .HasOne(lt => lt.ProcessedBy)
                .WithMany()
                .HasForeignKey(lt => lt.ProcessedById)
                .OnDelete(DeleteBehavior.Restrict);

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
        
        // New entities for tablet system
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableSession> TableSessions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemModifier> OrderItemModifiers { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
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
