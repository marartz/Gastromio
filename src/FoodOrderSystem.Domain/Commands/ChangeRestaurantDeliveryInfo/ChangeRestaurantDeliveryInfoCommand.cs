using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryInfo
{
    public class ChangeRestaurantDeliveryInfoCommand : ICommand<bool>
    {
        public ChangeRestaurantDeliveryInfoCommand(RestaurantId restaurantId, DeliveryInfo deliveryInfo)
        {
            RestaurantId = restaurantId;
            DeliveryInfo = deliveryInfo;
        }

        public RestaurantId RestaurantId { get; }
        
        public DeliveryInfo DeliveryInfo { get; }
    }
}
