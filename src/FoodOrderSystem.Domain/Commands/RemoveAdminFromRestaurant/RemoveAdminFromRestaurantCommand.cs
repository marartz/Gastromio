using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.RemoveAdminFromRestaurant
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
