using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.EnableSupportForRestaurant
{
    public class EnableSupportForRestaurantCommand : ICommand<bool>
    {
        public EnableSupportForRestaurantCommand(RestaurantId restaurantId)
        {
            this.RestaurantId = restaurantId;
        }
        
        public RestaurantId RestaurantId { get; }
    }
}