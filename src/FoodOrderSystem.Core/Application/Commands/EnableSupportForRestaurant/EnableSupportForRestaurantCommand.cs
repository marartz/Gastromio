using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.EnableSupportForRestaurant
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