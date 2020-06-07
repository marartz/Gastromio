using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public interface IDishCategoryRepository
    {
        Task<IEnumerable<DishCategory>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<DishCategory> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken = default);

        Task StoreAsync(DishCategory dishCategory, CancellationToken cancellationToken = default);

        Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task RemoveAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken = default);
    }
}
