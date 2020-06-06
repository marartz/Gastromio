using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class DishCategoryRepository : IDishCategoryRepository
    {
        private readonly SystemDbContext dbContext;

        public DishCategoryRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<IEnumerable<DishCategory>> FindByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var restaurantRow = dbContext.Restaurants.FirstOrDefault(en => en.Id == restaurantId.Value);
                if (restaurantRow == null)
                    return null;
                return (IEnumerable<DishCategory>) restaurantRow.Categories
                    .OrderBy(en => en.OrderNo)
                    .ThenBy(en => en.Name)
                    .Select(en => FromRow(en));
            }, cancellationToken);
        }

        public Task<DishCategory> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.DishCategories.FirstOrDefault(en => en.Id == dishCategoryId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task StoreAsync(DishCategory dishCategory, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.DishCategories;

                var row = dbSet.FirstOrDefault(x => x.Id == dishCategory.Id.Value);
                if (row != null)
                {
                    ToRow(dishCategory, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new DishCategoryRow();
                    ToRow(dishCategory, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.DishCategories;

                var rows = dbSet.Where(en => en.RestaurantId == restaurantId.Value);
                foreach (var row in rows)
                {
                    dbSet.Remove(row);
                }
                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.DishCategories;

                var row = dbSet.FirstOrDefault(en => en.Id == dishCategoryId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }

        private static DishCategory FromRow(DishCategoryRow row)
        {
            return new DishCategory(new DishCategoryId(row.Id),
                new RestaurantId(row.RestaurantId),
                row.Name,
                row.OrderNo
            );
        }

        private static void ToRow(DishCategory obj, DishCategoryRow row)
        {
            row.Id = obj.Id.Value;
            row.RestaurantId = obj.RestaurantId.Value;
            row.Name = obj.Name;
            row.OrderNo = obj.OrderNo;
        }
    }
}