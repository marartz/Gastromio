using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.EnableDishCategory
{
    public class EnableDishCategoryCommand : ICommand<bool>
    {
        public EnableDishCategoryCommand(RestaurantId restaurantId, DishCategoryId categoryId)
        {
            RestaurantId = restaurantId;
            CategoryId = categoryId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
    }
}
