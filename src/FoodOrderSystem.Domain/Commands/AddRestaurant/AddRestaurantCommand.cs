namespace FoodOrderSystem.Domain.Commands.AddRestaurant
{
    public class AddRestaurantCommand : ICommand
    {
        public AddRestaurantCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
