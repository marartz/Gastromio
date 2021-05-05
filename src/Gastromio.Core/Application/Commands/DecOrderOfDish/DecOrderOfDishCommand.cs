using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.DecOrderOfDish
{
    public class DecOrderOfDishCommand : ICommand<bool>
    {
        public DecOrderOfDishCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId, DishId dishId)
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
