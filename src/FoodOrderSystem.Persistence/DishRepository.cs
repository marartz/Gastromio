using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class DishRepository : IDishRepository
    {
        private readonly SystemDbContext dbContext;

        public DishRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ICollection<Dish>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var restaurantRow = dbContext.Restaurants.FirstOrDefault(en => en.Id == restaurantId.Value);
                if (restaurantRow == null)
                    return null;
                return (ICollection<Dish>)restaurantRow.Dishes
                    .OrderBy(en => en.OrderNo)
                    .ThenBy(en => en.Name)
                    .Select(en => FromRow(en))
                    .ToList();
            }, cancellationToken);
        }

        public Task<ICollection<Dish>> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var dishCategoryRow = dbContext.DishCategories.FirstOrDefault(en => en.Id == dishCategoryId.Value);
                if (dishCategoryRow == null)
                    return null;
                return (ICollection<Dish>)dishCategoryRow.Dishes
                    .OrderBy(en => en.Name)
                    .Select(en => FromRow(en))
                    .ToList();
            }, cancellationToken);
        }

        public Task<Dish> FindByDishIdAsync(DishId dishId, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Dishes.FirstOrDefault(en => en.Id == dishId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task StoreAsync(Dish dish, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Dishes;

                var row = dbSet.FirstOrDefault(x => x.Id == dish.Id.Value);
                if (row != null)
                {
                    ToRow(dish, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new DishRow();
                    ToRow(dish, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(DishId dishId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Dishes;

                var row = dbSet.FirstOrDefault(en => en.Id == dishId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }

        private static Dish FromRow(DishRow row)
        {
            return new Dish(
                new DishId(row.Id),
                new RestaurantId(row.RestaurantId),
                new DishCategoryId(row.CategoryId),
                row.Name,
                row.Description,
                row.ProductInfo,
                row.OrderNo,
                row.Variants != null ? row.Variants
                    .Select(variantRow => new DishVariant(
                        variantRow.VariantId,
                        variantRow.Name,
                        variantRow.Price,
                        variantRow.Extras != null ? variantRow.Extras
                            .Select(extraRow => new DishVariantExtra(
                                extraRow.ExtraId,
                                extraRow.Name,
                                extraRow.ProductInfo,
                                extraRow.Price)
                            ).ToList() : new List<DishVariantExtra>()
                    )).ToList() : new List<DishVariant>()
            );
        }

        private static void ToRow(Dish obj, DishRow row)
        {
            row.Id = obj.Id.Value;
            row.RestaurantId = obj.RestaurantId.Value;
            row.CategoryId = obj.CategoryId.Value;
            row.Name = obj.Name;
            row.Description = obj.Description;
            row.ProductInfo = obj.ProductInfo;
            row.OrderNo = obj.OrderNo;
            row.Variants = obj.Variants
                .Select(variant => new DishVariantRow()
                {
                    DishId = obj.Id.Value,
                    VariantId = variant.VariantId,
                    Name = variant.Name,
                    Price = variant.Price,
                    Extras = new List<DishVariantExtraRow>()
                }).ToList();
        }
    }
}
