using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Application.Commands.AddAdminToRestaurant
{
    public class AddAdminToRestaurantCommand : ICommand<bool>
    {
        public AddAdminToRestaurantCommand(RestaurantId restaurantId, UserId userId)
        {
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public RestaurantId RestaurantId { get; }
        public UserId UserId { get; }
    }
}
