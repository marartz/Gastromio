using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.AddRestaurant
{
    public class AddRestaurantCommand : ICommand<RestaurantViewModel>
    {
        public AddRestaurantCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
