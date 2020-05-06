using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.RemoveDishFromRestaurant
{
    public class RemoveDishFromRestaurantCommand : ICommand<bool>
    {
        public RemoveDishFromRestaurantCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId, DishId dishId)
        {
            RestaurantId = restaurantId;
            DishCategoryId = dishCategoryId;
            DishId = dishId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId DishCategoryId { get; }
        public DishId DishId { get; }
    }
}
