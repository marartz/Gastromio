using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.DisableSupportForRestaurant
{
    public class DisableSupportForRestaurantCommand : ICommand<bool>
    {
        public DisableSupportForRestaurantCommand(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }
        
        public RestaurantId RestaurantId { get; }
    }
}