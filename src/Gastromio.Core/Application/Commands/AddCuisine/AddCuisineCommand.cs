using Gastromio.Core.Application.DTOs;

namespace Gastromio.Core.Application.Commands.AddCuisine
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
