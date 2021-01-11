using Gastromio.Core.Domain.Model.Dish;
using Gastromio.Core.Domain.Model.DishCategory;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.RemoveDishFromRestaurant
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
