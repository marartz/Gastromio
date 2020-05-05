using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public interface IDishRepository
    {
        Task<ICollection<Dish>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken);

        Task<ICollection<Dish>> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken);

        Task<Dish> FindByDishIdAsync(DishId dishId, CancellationToken cancellationToken);

        Task StoreAsync(Dish dish, CancellationToken cancellationToken = default);

        Task RemoveAsync(DishId dishId, CancellationToken cancellationToken = default);
    }
}
