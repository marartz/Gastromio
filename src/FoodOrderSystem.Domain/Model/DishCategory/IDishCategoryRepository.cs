using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public interface IDishCategoryRepository
    {
        Task<ICollection<DishCategory>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken);
        Task<DishCategory> FindByCategoryIdAsync(DishCategoryId categoryId, CancellationToken cancellationToken);
    }
}
