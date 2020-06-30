using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.DeactivateRestaurant
{
    public class DeactivateRestaurantCommand : ICommand<bool>
    {
        public DeactivateRestaurantCommand(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }
        
        public RestaurantId RestaurantId { get; }
    }
}