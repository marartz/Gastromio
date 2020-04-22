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

        public Task<ICollection<DishCategory>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var restaurantRow = dbContext.Restaurants.FirstOrDefault(en => en.Id == restaurantId.Value);
                if (restaurantRow == null)
                    return null;
                return (ICollection<DishCategory>)restaurantRow.Categories.OrderBy(en => en.Name).Select(en => FromRow(en)).ToList();
            }, cancellationToken);
        }

        public Task<DishCategory> FindByCategoryIdAsync(DishCategoryId categoryId, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Categories.FirstOrDefault(en => en.Id == categoryId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        private static DishCategory FromRow(DishCategoryRow row)
        {
            return new DishCategory(new DishCategoryId(row.Id),
                new RestaurantId(row.RestaurantId),
                row.Name
            );
        }

        private static DishCategoryRow ToRow(DishCategory obj)
        {
            return new DishCategoryRow
            {
                Id = obj.CategoryId.Value,
                RestaurantId = obj.RestaurantId.Value,
                Name = obj.Name,
            };
        }
    }
}
