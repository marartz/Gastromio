using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.AddCuisine
{
    public class AddCuisineCommand : ICommand<CuisineViewModel>
    {
        public AddCuisineCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
