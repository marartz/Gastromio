using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.RemoveDishCategoryFromRestaurant
{
    public class RemoveDishCategoryFromRestaurantCommand : ICommand<bool>
    {
        public RemoveDishCategoryFromRestaurantCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId)
        {
            RestaurantId = restaurantId;
            DishCategoryId = dishCategoryId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId DishCategoryId { get; }
    }
}
