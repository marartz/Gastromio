using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.DecOrderOfDishCategory
{
    public class DecOrderOfDishCategoryCommand : ICommand<bool>
    {
        public DecOrderOfDishCategoryCommand(RestaurantId restaurantId, DishCategoryId categoryId)
        {
            RestaurantId = restaurantId;
            CategoryId = categoryId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
    }
}
