using Gastromio.Core.Application.DTOs;

namespace Gastromio.Core.Application.Commands.AddCuisine
{
    public class AddCuisineCommand : ICommand<CuisineDTO>
    {
        public AddCuisineCommand(string name, string image)
        {
            Name = name;
            Image = image;
        }

        public string Name { get; }
        
        public string Image { get; }
    }
}
