namespace FoodOrderSystem.Domain.Commands.AddCuisine
{
    public class AddCuisineCommand : ICommand
    {
        public AddCuisineCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
