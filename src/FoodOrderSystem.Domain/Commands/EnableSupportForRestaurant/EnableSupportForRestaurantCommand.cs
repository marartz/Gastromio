using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.EnableSupportForRestaurant
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