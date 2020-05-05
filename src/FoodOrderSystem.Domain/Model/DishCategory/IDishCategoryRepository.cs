using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public interface IDishCategoryRepository
    {
        Task<ICollection<DishCategory>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken);

        Task<DishCategory> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken);

        Task StoreAsync(DishCategory dishCategory, CancellationToken cancellationToken = default);

        Task RemoveAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken = default);
    }
}
