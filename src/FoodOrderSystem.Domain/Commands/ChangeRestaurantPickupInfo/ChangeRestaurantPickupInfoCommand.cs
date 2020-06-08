using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantPickupInfo
{
    public class ChangeRestaurantPickupInfoCommand : ICommand<bool>
    {
        public ChangeRestaurantPickupInfoCommand(RestaurantId restaurantId, PickupInfo pickupInfo)
        {
            RestaurantId = restaurantId;
            PickupInfo = pickupInfo;
        }

        public RestaurantId RestaurantId { get; }
        
        public PickupInfo PickupInfo { get; }
    }
}
