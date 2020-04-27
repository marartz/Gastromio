using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantName
{
    public class ChangeRestaurantNameCommand : ICommand
    {
        public ChangeRestaurantNameCommand(RestaurantId restaurantId, string name)
        {
            RestaurantId = restaurantId;
            Name = name;
        }

        public RestaurantId RestaurantId { get; }
        public string Name { get; }
    }
}
