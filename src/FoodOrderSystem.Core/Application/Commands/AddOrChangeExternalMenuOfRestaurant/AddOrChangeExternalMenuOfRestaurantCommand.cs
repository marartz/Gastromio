using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddOrChangeExternalMenuOfRestaurant
{
    public class AddOrChangeExternalMenuOfRestaurantCommand : ICommand<bool>
    {
        public AddOrChangeExternalMenuOfRestaurantCommand(RestaurantId restaurantId, ExternalMenu externalMenu)
        {
            RestaurantId = restaurantId;
            ExternalMenu = externalMenu;
        }
        
        public RestaurantId RestaurantId { get; }

        public ExternalMenu ExternalMenu { get; }
    }
}