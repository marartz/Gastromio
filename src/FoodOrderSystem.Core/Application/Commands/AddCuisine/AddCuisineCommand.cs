using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Commands.AddCuisine
{
    public class AddCuisineCommand : ICommand<CuisineDTO>
    {
        public AddCuisineCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
