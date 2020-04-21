using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystem.Persistence
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }

        public DbSet<UserRow> Users { get; set; }
        public DbSet<PaymentMethodRow> PaymentMethods { get; set; }
        public DbSet<RestaurantRow> Restaurants { get; set; }
        public DbSet<DishCategoryRow> Categories { get; set; }
        public DbSet<DishRow> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryTimeRow>()
                .HasKey(dt => new { dt.RestaurantId, dt.DayOfWeek, dt.StartTime });

            modelBuilder.Entity<DeliveryTimeRow>()
                .HasOne(dt => dt.Restaurant)
                .WithMany(s => s.DeliveryTimes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(dt => dt.RestaurantId);

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasKey(spm => new { spm.RestaurantId, spm.PaymentMethodId });

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(spm => spm.Restaurant)
                .WithMany(s => s.RestaurantPaymentMethods)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(spm => spm.RestaurantId);

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(spm => spm.PaymentMethod)
                .WithMany(pm => pm.RestaurantPaymentMethods)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(spm => spm.PaymentMethodId);

            modelBuilder.Entity<DishCategoryRow>()
                .HasOne(c => c.Restaurant)
                .WithMany(s => s.Categories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(c => c.RestaurantId);

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Restaurant)
                .WithMany(s => s.Dishes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(d => d.RestaurantId);

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Category)
                .WithMany(s => s.Dishes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(d => d.CategoryId);

            modelBuilder.Entity<DishVariantRow>()
                .HasKey(dv => new { dv.DishId, dv.Name });

            modelBuilder.Entity<DishVariantRow>()
                .HasOne(dv => dv.Dish)
                .WithMany(d => d.Variants)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(dv => dv.DishId);

            modelBuilder.Entity<DishVariantExtraRow>()
                .HasKey(dve => new { dve.DishId, dve.VariantName, dve.Name });

            modelBuilder.Entity<DishVariantExtraRow>()
                .HasOne(dve => dve.Variant)
                .WithMany(dv => dv.Extras)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(dve => new { dve.DishId, dve.VariantName });
        }
    }
}
