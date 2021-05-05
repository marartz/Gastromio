using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.IncOrderOfDish
{
    public class IncOrderOfDishCommand : ICommand<bool>
    {
        public IncOrderOfDishCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId, DishId dishId)
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
