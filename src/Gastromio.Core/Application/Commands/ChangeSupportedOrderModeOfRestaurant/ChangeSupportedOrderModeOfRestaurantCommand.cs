using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.ChangeSupportedOrderModeOfRestaurant
{
    public class ChangeSupportedOrderModeOfRestaurantCommand : ICommand
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
