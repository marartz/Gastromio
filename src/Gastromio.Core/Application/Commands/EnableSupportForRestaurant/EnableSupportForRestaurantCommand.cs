using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.EnableSupportForRestaurant
{
    public class EnableSupportForRestaurantCommand : ICommand
    {
        public EnableSupportForRestaurantCommand(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
