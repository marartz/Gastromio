using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.AddAdminToRestaurant
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
