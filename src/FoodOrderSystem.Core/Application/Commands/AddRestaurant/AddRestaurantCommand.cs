using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Commands.AddRestaurant
{
    public class AddRestaurantCommand : ICommand<RestaurantDTO>
    {
        public AddRestaurantCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
