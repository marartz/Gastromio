using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ActivateRestaurant
{
    public class ActivateRestaurantCommand : ICommand<bool>
    {
        public ActivateRestaurantCommand(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}