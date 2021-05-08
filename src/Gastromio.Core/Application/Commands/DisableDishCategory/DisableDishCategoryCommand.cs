using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.DisableDishCategory
{
    public class DisableDishCategoryCommand : ICommand
    {
        public DisableDishCategoryCommand(RestaurantId restaurantId, DishCategoryId categoryId)
        {
            RestaurantId = restaurantId;
            CategoryId = categoryId;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
    }
}
