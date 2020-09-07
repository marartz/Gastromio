using FoodOrderSystem.Core.Domain.Model.DishCategory;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveDishCategoryFromRestaurant
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
