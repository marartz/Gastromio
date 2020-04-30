using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantName
{
    public class ChangeRestaurantNameCommand : ICommand<bool>
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
