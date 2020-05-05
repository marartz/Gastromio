using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystem.Persistence
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }

        public DbSet<UserRow> Users { get; set; }
        public DbSet<CuisineRow> Cuisines { get; set; }
        public DbSet<PaymentMethodRow> PaymentMethods { get; set; }
        public DbSet<RestaurantRow> Restaurants { get; set; }
        public DbSet<DishCategoryRow> DishCategories { get; set; }
        public DbSet<DishRow> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RestaurantUserRow>()
                .HasKey(rpm => new { rpm.RestaurantId, rpm.UserId });

            modelBuilder.Entity<RestaurantUserRow>()
                .HasOne(rpm => rpm.Restaurant)
                .WithMany(r => r.RestaurantUsers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rpm => rpm.RestaurantId);

            modelBuilder.Entity<RestaurantUserRow>()
                .HasOne(rpm => rpm.User)
                .WithMany(pm => pm.RestaurantUsers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rpm => rpm.UserId);

            modelBuilder.Entity<DeliveryTimeRow>()
                .HasKey(dt => new { dt.RestaurantId, dt.DayOfWeek, dt.StartTime });

            modelBuilder.Entity<DeliveryTimeRow>()
                .HasOne(dt => dt.Restaurant)
                .WithMany(r => r.DeliveryTimes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(dt => dt.RestaurantId);

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasKey(rc => new { rc.RestaurantId, rc.CuisineId });

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasOne(rc => rc.Restaurant)
                .WithMany(r => r.RestaurantCuisines)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rc => rc.RestaurantId);

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasOne(rc => rc.Cuisine)
                .WithMany(c => c.RestaurantCuisines)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rc => rc.CuisineId);

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasKey(rpm => new { rpm.RestaurantId, rpm.PaymentMethodId });

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(rpm => rpm.Restaurant)
                .WithMany(r => r.RestaurantPaymentMethods)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rpm => rpm.RestaurantId);

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(rpm => rpm.PaymentMethod)
                .WithMany(pm => pm.RestaurantPaymentMethods)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(rpm => rpm.PaymentMethodId);

            modelBuilder.Entity<DishCategoryRow>()
                .HasOne(dc => dc.Restaurant)
                .WithMany(r => r.Categories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(dc => dc.RestaurantId);

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Restaurant)
                .WithMany(r => r.Dishes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(d => d.RestaurantId);

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Category)
                .WithMany(dc => dc.Dishes)
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
