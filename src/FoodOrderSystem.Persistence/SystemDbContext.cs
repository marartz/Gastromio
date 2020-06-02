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
                .HasForeignKey(rpm => rpm.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<RestaurantUserRow>()
                .HasOne(rpm => rpm.User)
                .WithMany(pm => pm.RestaurantUsers)
                .HasForeignKey(rpm => rpm.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DeliveryTimeRow>()
                .HasKey(dt => new { dt.RestaurantId, dt.DayOfWeek, dt.StartTime });

            modelBuilder.Entity<DeliveryTimeRow>()
                .HasOne(dt => dt.Restaurant)
                .WithMany(r => r.DeliveryTimes)
                .HasForeignKey(dt => dt.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasKey(rc => new { rc.RestaurantId, rc.CuisineId });

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasOne(rc => rc.Restaurant)
                .WithMany(r => r.RestaurantCuisines)
                .HasForeignKey(rc => rc.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<RestaurantCuisineRow>()
                .HasOne(rc => rc.Cuisine)
                .WithMany(c => c.RestaurantCuisines)
                .HasForeignKey(rc => rc.CuisineId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasKey(rpm => new { rpm.RestaurantId, rpm.PaymentMethodId });

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(rpm => rpm.Restaurant)
                .WithMany(r => r.RestaurantPaymentMethods)
                .HasForeignKey(rpm => rpm.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<RestaurantPaymentMethodRow>()
                .HasOne(rpm => rpm.PaymentMethod)
                .WithMany(pm => pm.RestaurantPaymentMethods)
                .HasForeignKey(rpm => rpm.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DishCategoryRow>()
                .HasOne(dc => dc.Restaurant)
                .WithMany(r => r.Categories)
                .HasForeignKey(dc => dc.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Restaurant)
                .WithMany(r => r.Dishes)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DishRow>()
                .HasOne(d => d.Category)
                .WithMany(dc => dc.Dishes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DishVariantRow>()
                .HasKey(dv => new { dv.DishId, dv.VariantId });

            modelBuilder.Entity<DishVariantRow>()
                .HasOne(dv => dv.Dish)
                .WithMany(d => d.Variants)
                .HasForeignKey(dv => dv.DishId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<DishVariantExtraRow>()
                .HasKey(dve => new { dve.DishId, dve.VariantId, dve.ExtraId });

            modelBuilder.Entity<DishVariantExtraRow>()
                .HasOne(dve => dve.Variant)
                .WithMany(dv => dv.Extras)
                .HasForeignKey(dve => new { dve.DishId, dve.VariantId })
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
