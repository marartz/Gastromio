using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantAddress
{
    public class ChangeRestaurantAddressCommand : ICommand<bool>
    {
        public ChangeRestaurantAddressCommand(RestaurantId restaurantId, Address address)
        {
            RestaurantId = restaurantId;
            Address = address;
        }

        public RestaurantId RestaurantId { get; }
        public Address Address { get; }
    }
}
