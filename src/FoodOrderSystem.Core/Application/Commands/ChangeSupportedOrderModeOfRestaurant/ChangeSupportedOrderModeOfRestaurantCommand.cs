using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.ChangeSupportedOrderModeOfRestaurant
{
    public class ChangeSupportedOrderModeOfRestaurantCommand : ICommand<bool>
    {
        public ChangeSupportedOrderModeOfRestaurantCommand(RestaurantId restaurantId,
            SupportedOrderMode supportedOrderMode)
        {
            RestaurantId = restaurantId;
            SupportedOrderMode = supportedOrderMode;
        }
        
        public RestaurantId RestaurantId { get; }

        public SupportedOrderMode SupportedOrderMode { get; }
    }
}