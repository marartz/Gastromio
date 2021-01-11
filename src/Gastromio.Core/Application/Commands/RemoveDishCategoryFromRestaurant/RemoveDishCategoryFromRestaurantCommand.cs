using Gastromio.Core.Domain.Model.DishCategory;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.RemoveDishCategoryFromRestaurant
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
