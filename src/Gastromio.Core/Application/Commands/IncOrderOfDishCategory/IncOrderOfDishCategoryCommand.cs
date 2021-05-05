using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.IncOrderOfDishCategory
{
    public class IncOrderOfDishCategoryCommand : ICommand<bool>
    {
        public IncOrderOfDishCategoryCommand(RestaurantId restaurantId, DishCategoryId categoryId)
        {
            RestaurantId = restaurantId;
            CategoryId = categoryId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
    }
}
