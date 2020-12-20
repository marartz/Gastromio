using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Application.Commands.RemoveAdminFromRestaurant
{
    public class RemoveAdminFromRestaurantCommand : ICommand<bool>
    {
        public RemoveAdminFromRestaurantCommand(RestaurantId restaurantId, UserId userId)
        {
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public RestaurantId RestaurantId { get; }
        public UserId UserId { get; }
    }
}
