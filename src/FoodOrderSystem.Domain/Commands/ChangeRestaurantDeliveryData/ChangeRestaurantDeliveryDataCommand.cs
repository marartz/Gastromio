using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryData
{
    public class ChangeRestaurantDeliveryDataCommand : ICommand
    {
        public ChangeRestaurantDeliveryDataCommand(RestaurantId restaurantId, decimal minimumOrderValue, decimal deliveryCosts)
        {
            RestaurantId = restaurantId;
            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;
        }

        public RestaurantId RestaurantId { get; }
        public decimal MinimumOrderValue { get; }
        public decimal DeliveryCosts { get; }
    }
}
